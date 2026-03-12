using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace TransactionIngest.Tests;

[TestClass]
public class AddTransactionProcessorTests
{
    private static TransactionDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TransactionDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TransactionDbContext(options);
    }

    [TestMethod]
    public void AddTransactions()
    {
        var db = CreateInMemoryDbContext();

        var logger = new Mock<ILogger<AddTransactionProcessor>>();
        var processor = new AddTransactionProcessor(logger.Object);

        var incomingTransactions = new List<Transaction>
        {
            new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false),
            new(2, "2222222222222222", "STO-02", "Dell Wireless Mouse-WM126", 14.99m, new DateTime(2026, 1, 2), false),
        };

        processor.AddTransactions(db, incomingTransactions);
        Assert.AreEqual(2, db.Transactions.Count());
    }

    [TestMethod]
    public void AddTransactions_DoesNotAddDuplicates()
    {
        var db = CreateInMemoryDbContext();

        db.Transactions.Add(
           new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false)
        );
        db.SaveChanges();

        var loggerMock = new Mock<ILogger<AddTransactionProcessor>>();
        var processor = new AddTransactionProcessor(loggerMock.Object);

        var incoming = new List<Transaction>
        {
            new(1, "1111111111111111", "STO-01", "S2725H 27\" IPS LED FHD 100Hz Monitor", 109.99m, new DateTime(2026, 1, 1), false)
        };

        processor.AddTransactions(db, incoming);
        Assert.AreEqual(1, db.Transactions.Count());
    }
}