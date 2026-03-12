namespace TransactionIngest.Models;

public class TransactionChange(int transactionId, FieldChange fieldName, string oldValue, string newValue, DateTime transactionChangeTime)
{
    public int Id { get; set; }
    public int TransactionId { get; set; } = transactionId;
    public FieldChange FieldName { get; set; } = fieldName;
    public string OldValue { get; set; } = oldValue;
    public string NewValue { get; set; } = newValue;
    public DateTime TransactionChangeTime { get; set; } = transactionChangeTime;
}

public enum FieldChange
{
    CardNumber,
    LocationCode,
    ProductName,
    Amount,
    TransactionTime,
    IsRevoked
}