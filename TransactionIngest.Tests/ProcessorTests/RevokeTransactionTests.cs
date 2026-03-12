using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TransactionIngest.Tests.ProcessorTests;

[TestClass]
public class RevokeTransactionsProcessorTests
{
    private static TransactionDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TransactionDbContext(options);
    }

    [TestMethod]
    public void RevokeTransactions()
    {
        var db = CreateInMemoryDbContext();

        var logger = new Mock<ILogger<RevokeTransactionProcessor>>();
        var processor = new RevokeTransactionProcessor(logger.Object);

        db.Transactions.AddRange(
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
           new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 1), false)
        );
        db.SaveChanges();

        var incomingTransactions = new List<Transaction>
        {
            new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 1), false)
        };

        processor.RevokeTransactions(db, incomingTransactions, new DateTime(2026, 1, 1));

        Assert.AreEqual(2, db.Transactions.Count());

        var firstTransaction = db.Transactions.First();    
        Assert.IsTrue(firstTransaction.IsRevoked);
        Assert.IsNotEmpty(firstTransaction.TransactionChanges);
        var transactionChange = firstTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.IsRevoked, transactionChange.FieldName);
        Assert.AreEqual("False", transactionChange.OldValue);
        Assert.AreEqual("True", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 1, 1), transactionChange.TransactionChangeTime);


        var secondTransaction = db.Transactions.Last();
        Assert.IsFalse(secondTransaction.IsRevoked);
        Assert.IsEmpty(secondTransaction.TransactionChanges);
    }

    [TestMethod]
    public void RevokeNoTransactions()
    {
        var db = CreateInMemoryDbContext();

        var logger = new Mock<ILogger<RevokeTransactionProcessor>>();
        var processor = new RevokeTransactionProcessor(logger.Object);

        db.Transactions.AddRange(
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
           new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 1), false)
        );
        db.SaveChanges();

        var incomingTransactions = new List<Transaction>
        {
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
           new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 1), false)
        };

        processor.RevokeTransactions(db, incomingTransactions, new DateTime(2026, 1, 1));
        Assert.AreEqual(2, db.Transactions.Count());

        var firstTransaction = db.Transactions.First();
        Assert.IsFalse(firstTransaction.IsRevoked);
        Assert.IsEmpty(firstTransaction.TransactionChanges);

        var secondTransaction = db.Transactions.Last();
        Assert.IsFalse(secondTransaction.IsRevoked);
        Assert.IsEmpty(secondTransaction.TransactionChanges);
    }
}
