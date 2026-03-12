using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionIngest.Models;

public interface IWorker
{
    public void Run(DateTime now);
}

public class Worker(ILogger<Worker> logger, IUpdateTransactionProcessor updateTransactionProcessor) : IWorker
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IUpdateTransactionProcessor _transactionProcessor = updateTransactionProcessor;

    public void Run(DateTime now)
    {
        using var db = new TransactionDbContext();
        var timeCutoff = now.AddHours(-24);

        _logger.LogInformation("Starting...");
        var timeStamp = now.ToString("yyyyMMdd_HHmmss");

        var json = File.ReadAllText("Data\\MockTransactions.json");
        List<Transaction>? incomingTransactions = JsonSerializer.Deserialize<List<Transaction>>(json);

        if (incomingTransactions == null || incomingTransactions.Count == 0)
        {
            _logger.LogInformation("No transactions to process.");
            return;
        }

        _logger.LogInformation("Adding new transactions...");
        AddNewTransactions(db, incomingTransactions);

        //_transactionProcessor.ProcessTransactions();

        _logger.LogInformation("Finished.");
    }

    public void AddNewTransactions(TransactionDbContext db, List<Transaction> incomingTransactions)
    {
        var newTransactions = new List<Transaction>();
        foreach (var transaction in incomingTransactions)
        {
            if (!db.Transactions.Any(t => t.TransactionId == transaction.TransactionId))
            {
                _logger.LogInformation("Adding transaction {TransactionId} to database...", transaction.TransactionId);
                newTransactions.Add(transaction);
            }
        }

        if (!newTransactions.Any())
        {
            _logger.LogInformation("No new transactions to add.");
            return;
        }

        db.AddRange(newTransactions);
        db.SaveChanges();

        _logger.LogInformation("All new transactions have been added.");
    }
}