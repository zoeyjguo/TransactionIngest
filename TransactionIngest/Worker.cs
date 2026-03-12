using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace TransactionIngest;

public interface IWorker
{
    public void Run(DateTime now);
}

public class Worker(ILogger<Worker> logger, IAddTransactionProcessor addTransactionProcessor, IUpdateTransactionProcessor updateTransactionProcessor, 
    IRevokeTransactionProcessor revokeTransactionProcessor, IServiceProvider serviceProvider, IOptions<ApiSettings> apiSettings) : IWorker
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly string _apiPath = apiSettings.Value.TransactionApiUrl;
    private readonly IUpdateTransactionProcessor _updateTransactionProcessor = updateTransactionProcessor;
    private readonly IAddTransactionProcessor _addTransactionProcessor = addTransactionProcessor;
    private readonly IRevokeTransactionProcessor _revokeTransactionProcessor = revokeTransactionProcessor;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void Run(DateTime now)
    {
        _logger.LogInformation("Starting...");

        var json = File.ReadAllText(_apiPath);
        _logger.LogInformation("Looking for transaction file at {ApiPath}", _apiPath);
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
        logger.LogInformation("All new transactions have been added.");

        _logger.LogInformation("Checking updated transactions...");
        _updateTransactionProcessor.UpdateTransactions(db, incomingTransactions, now);
        logger.LogInformation("All new transactions have been added.");

        _logger.LogInformation("Checking revoked transactions...");
        _revokeTransactionProcessor.RevokeTransactions(db, incomingTransactions, now);

        _logger.LogInformation("Finished.");
    }
}