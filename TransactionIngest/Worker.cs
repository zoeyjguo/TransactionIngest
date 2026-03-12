using Microsoft.Extensions.Logging;
using System.Text.Json;

public interface IWorker
{
    public void Run(DateTime now);
}

public class Worker(ILogger<Worker> logger, IAddTransactionProcessor addTransactionProcessor, IUpdateTransactionProcessor updateTransactionProcessor, 
    IRevokeTransactionProcessor revokeTransactionProcessor) : IWorker
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IUpdateTransactionProcessor _transactionProcessor = updateTransactionProcessor;
    private readonly IAddTransactionProcessor _addTransactionProcessor = addTransactionProcessor;
    private readonly IRevokeTransactionProcessor _revokeTransactionProcessor = revokeTransactionProcessor;

    public void Run(DateTime now)
    {
        using var db = new TransactionDbContext();

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
        _addTransactionProcessor.AddTransactions(db, incomingTransactions);

        _logger.LogInformation("Checking updated transactions...");
        _transactionProcessor.UpdateTransactions(db, incomingTransactions, now);

        _logger.LogInformation("Checking revoked transactions...");
        _revokeTransactionProcessor.RevokeTransactions(db, incomingTransactions, now);

        _logger.LogInformation("Finished.");
    }
}