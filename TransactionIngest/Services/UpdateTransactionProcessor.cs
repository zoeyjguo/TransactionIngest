namespace TransactionIngest.Services;

using Microsoft.Extensions.Logging;
using TransactionIngest.Models;

public interface IUpdateTransactionProcessor
{
    public void UpdateTransactions(TransactionDbContext db, List<Transaction> incomingTransactions, DateTime now);
}

public class UpdateTransactionProcessor(ILogger<UpdateTransactionProcessor> logger) : IUpdateTransactionProcessor
{
    public void UpdateTransactions(TransactionDbContext db, List<Transaction> incomingTransactions, DateTime now)
    {
        foreach (var transaction in incomingTransactions)
        {
            var existingTransaction = db.Transactions.FirstOrDefault(t => t.TransactionId == transaction.TransactionId);
            if (existingTransaction == null)
            {
                continue;
            }

            logger.LogInformation("Updating transaction {TransactionId}...", transaction.TransactionId);
            existingTransaction.UpdateFrom(transaction, now);
        }
        db.SaveChanges();

        logger.LogInformation("All transactions have been updated.");
    }
}
