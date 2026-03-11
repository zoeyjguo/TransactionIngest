namespace TransactionIngest.Models;

public class Transaction(int TransactionId, string CardNumber, string LocationCode, string ProductName, decimal Amount, DateTime TransactionTime)
{
    public int TransactionId { get; protected set; } = TransactionId;
    public string CardNumber { get; protected set; } = CardNumber;
    public string LocationCode { get; protected set; } = LocationCode;
    public string ProductName { get; protected set; } = ProductName;
    public decimal Amount { get; protected set; } = Amount;
    public DateTime TransactionTime { get; protected set; } = TransactionTime;
    public Status Status { get; protected set; } = Status.Active;
    public List<TransactionChange> TransactionChanges { get; protected set; } = [];
}

public enum Status
{
    Active,
    Revoked, 
    Finalized
}