using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TransactionIngest.Tests.ProcessorTests;

[TestClass]
public class UpdateTransactionProcessorTests
{
    private static TransactionDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TransactionDbContext(options);
    }

    [TestMethod]
    public void UpdateTransactions()
    {
        var db = CreateInMemoryDbContext();

        var logger = new Mock<ILogger<UpdateTransactionProcessor>>();
        var processor = new UpdateTransactionProcessor(logger.Object);

        db.Transactions.Add(
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false)
        );
        db.SaveChanges();

        var incomingTransactions = new List<Transaction>
        {
            new(1, "1111111111111111", "STO-02", "S2725H 27\" IPS LED FHD 100Hz Monitor", 50.99m, new DateTime(2026, 1, 2), false)
        };

        processor.UpdateTransactions(db, incomingTransactions, new DateTime(2026, 1, 10));
        Assert.AreEqual(1, db.Transactions.Count());

        var transaction = db.Transactions.First();
        Assert.HasCount(3, transaction.TransactionChanges);

        var firstChange = transaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.Amount, firstChange.FieldName);
        Assert.AreEqual("109.99", firstChange.OldValue);
        Assert.AreEqual("50.99", firstChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 1, 10), firstChange.TransactionChangeTime);

        var secondChange = transaction.TransactionChanges[1];
        Assert.AreEqual(FieldChange.LocationCode, secondChange.FieldName);
        Assert.AreEqual("STO-01", secondChange.OldValue);
        Assert.AreEqual("STO-02", secondChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 1, 10), secondChange.TransactionChangeTime);

        var thirdChange = transaction.TransactionChanges.Last();
        Assert.AreEqual(FieldChange.TransactionTime, thirdChange.FieldName);
        Assert.AreEqual(new DateTime(2026, 1, 1).ToString(), thirdChange.OldValue);
        Assert.AreEqual(new DateTime(2026, 1, 2).ToString(), thirdChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 1, 10), thirdChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateNoTransactions()
    {
        var db = CreateInMemoryDbContext();

        var logger = new Mock<ILogger<RevokeTransactionProcessor>>();
        var processor = new RevokeTransactionProcessor(logger.Object);

        db.Transactions.AddRange(
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
           new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 2), false)
        );
        db.SaveChanges();

        var incomingTransactions = new List<Transaction>
        {
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
           new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 2), false)
        };

        processor.RevokeTransactions(db, incomingTransactions, new DateTime(2026, 1, 10));
        Assert.AreEqual(2, db.Transactions.Count());

        var firstTransaction = db.Transactions.First();
        Assert.IsEmpty(firstTransaction.TransactionChanges);

        var secondTransaction = db.Transactions.Last();
        Assert.IsEmpty(secondTransaction.TransactionChanges);
    }
}
