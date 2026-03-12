namespace TransactionIngest.Services;

using Microsoft.Extensions.Logging;
using TransactionIngest.Models;

public interface IUpdateTransactionProcessor
{
    public void AddNewTransactions(List<Transaction> newTransactions);
    //public void UpdateTransactions();
}

public class UpdateTransactionProcessor(ILogger<UpdateTransactionProcessor> logger) : IUpdateTransactionProcessor
{
    //private TransactionDbContext db;
    //private DateTime timeCutoff;
    //private List<Transaction> incomingTransactions;

    //public UpdateTransactionProcessor(ILogger<UpdateTransactionProcessor> logger, List<Transaction> transactions, DateTime now)
    //{
    //    db = new TransactionDbContext();
    //    timeCutoff = now.AddHours(-24);
    //    incomingTransactions = transactions;
    //}

    //public void UpdateTransactions(Transaction transaction)
    //{
    //}
}
