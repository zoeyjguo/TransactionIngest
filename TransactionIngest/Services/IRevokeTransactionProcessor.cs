using Microsoft.Extensions.Logging;

namespace TransactionIngest.Services;

public interface IRevokeTransactionProcessor
{
    public void RevokeTransactions(TransactionDbContext db, List<Transaction> incomingTransactions, DateTime cutoff);
}

public class RevokeTransactionProcessor(ILogger<RevokeTransactionProcessor> logger) : IRevokeTransactionProcessor
{
    public void RevokeTransactions(TransactionDbContext db, List<Transaction> incomingTransactions, DateTime now)
    {
        var timeCutoff = now.AddHours(-24);
        var incomingTransactionIds = incomingTransactions.Select(t => t.TransactionId).ToHashSet();
        var transactionsWithinCutoff = db.Transactions.Where(t => t.TransactionTime >= timeCutoff).ToList();

        foreach (var transaction in transactionsWithinCutoff)
        {
            if (!incomingTransactionIds.Contains(transaction.TransactionId))
            {
                logger.LogInformation("Revoking transaction {TransactionId}...", transaction.TransactionId);
                transaction.RevokeTransaction(now);
            }
        }
        db.SaveChanges();

        logger.LogInformation("All transactions absent within the last 24 hours have been revoked.");
    }
}