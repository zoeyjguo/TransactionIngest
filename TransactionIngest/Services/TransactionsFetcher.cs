namespace TransactionIngest.Services;
using System.Text.Json;
using TransactionIngest.Models;

public interface ITransactionFetcher
{
    Task<List<TransactionDto>> GetTransactions();
}

public class TransactionFetcher : ITransactionFetcher
{
    public async Task<List<TransactionDto>> GetTransactions()
    {
        var json = await File.ReadAllTextAsync("..\\Data\\MockTransactions.json");
        return JsonSerializer.Deserialize<List<TransactionDto>>(json)!;
    }
}