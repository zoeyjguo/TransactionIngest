using Microsoft.Extensions.Logging;

namespace TransactionIngest.Services;

public interface IAddTransactionProcessor
{
    public void AddTransactions(TransactionDbContext db, List<Transaction> incomingTransactions);
}

public class AddTransactionProcessor(ILogger<AddTransactionProcessor> logger) : IAddTransactionProcessor
{
    public void AddTransactions(TransactionDbContext db, List<Transaction> incomingTransactions)
    {
        var newTransactions = new List<Transaction>();
        var existingIds = db.Transactions.Select(t => t.TransactionId).ToHashSet();

        foreach (var transaction in incomingTransactions)
        {
            if (!existingIds.Contains(transaction.TransactionId))
            {
                logger.LogInformation("Adding transaction {TransactionId} to database...", transaction.TransactionId);
                newTransactions.Add(transaction);
            }
        }

        if (!newTransactions.Any())
        {
            logger.LogInformation("No new transactions to add.");
            return;
        }

        db.AddRange(newTransactions);
        db.SaveChanges();

        logger.LogInformation("All new transactions have been added.");
    }
}