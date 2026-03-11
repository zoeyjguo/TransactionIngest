namespace TransactionIngest.Services;
using TransactionIngest.Models;

public interface ITransactionProcessor
{
    public Task ProcessTransactionsAsync(List<Transaction> transactions, DateTime now);
    public Task AddNewTransactionsAsync();
    public void ChangeTransaction(Transaction transaction);
}

public class TransactionProcessor(TransactionDbContext context, ITransactionFetcher fetcher) : ITransactionProcessor
{
    private readonly TransactionDbContext _context = context;
    private readonly ITransactionFetcher _fetcher = fetcher;

    public async Task ProcessTransactionsAsync(List<Transaction> transactions, DateTime now)
    {
        var timeCutoff = now.AddHours(-24);

        await AddNewTransactionsAsync();
    }

    public async Task AddNewTransactionsAsync()
    {
        var newTransactions = await _fetcher.GetTransactions();

        foreach (var transaction in newTransactions)
        {
            if (!_context.Transactions.Any(t => t.TransactionId == transaction.TransactionId))
            {
                _context.Transactions.Add(new Transaction(
                    transaction.TransactionId,
                    transaction.CardNumber,
                    transaction.LocationCode,
                    transaction.ProductName,
                    transaction.Amount,
                    transaction.TransactionTime
                    ));

            }
        }
    }

    public void ChangeTransaction(Transaction transaction)
    {
    }
}
