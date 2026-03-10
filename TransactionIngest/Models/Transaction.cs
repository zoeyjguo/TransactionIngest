namespace TransactionIngest.Models;

public class Transaction
{
    public int TransactionId { get; protected set; }
    public string CardNumber { get; protected set; } = string.Empty;
    public string LocationCode { get; protected set; } = string.Empty;
    public string ProductName { get; protected set; } = string.Empty;
    public decimal Amount { get; protected set; }
    public DateTime TransactionTime { get; protected set; }
    public Status Status { get; protected set; } = Status.Finalized;
}

public enum Status
{
    Revoked, 
    Finalized
}