using Microsoft.Extensions.Logging;
using System.Text.Json;

public interface IWorker
{
    public void Run(DateTime now);
}

public class Worker(ILogger<Worker> logger, IAddTransactionProcessor addTransactionProcessor, IUpdateTransactionProcessor updateTransactionProcessor, 
    RevokeTransactionProcessor revokeTransactionProcessor, IServiceProvider serviceProvider) : IWorker
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IUpdateTransactionProcessor _transactionProcessor = updateTransactionProcessor;
    private readonly IAddTransactionProcessor _addTransactionProcessor = addTransactionProcessor;
    private readonly RevokeTransactionProcessor _revokeTransactionProcessor = revokeTransactionProcessor;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void Run(DateTime now)
    {
        _logger.LogInformation("Starting...");
        var timeStamp = now.ToString("yyyyMMdd_HHmmss");

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var jsonFile = Path.Combine(baseDir, "Data", "MockTransactions.json");
        _logger.LogInformation("Looking for transaction file at {JsonPath}", jsonFile);
        var json = File.ReadAllText(jsonFile);
        List<Transaction>? incomingTransactions = JsonSerializer.Deserialize<List<Transaction>>(json);

        if (incomingTransactions == null || incomingTransactions.Count == 0)
        {
            _logger.LogInformation("No transactions to process.");
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
        db.Database.EnsureCreated();

        _logger.LogInformation("Adding new transactions...");
        _addTransactionProcessor.AddTransactions(db, incomingTransactions);

        _logger.LogInformation("Checking updated transactions...");
        _transactionProcessor.UpdateTransactions(db, incomingTransactions, now);

        _logger.LogInformation("Checking revoked transactions...");
        _revokeTransactionProcessor.RevokeTransactions(db, incomingTransactions, now);

        _logger.LogInformation("Finished.");
    }
}