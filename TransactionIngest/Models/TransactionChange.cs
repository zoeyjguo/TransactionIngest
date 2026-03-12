namespace TransactionIngest.Models;

public class TransactionChange(int TransactionId, FieldChange FieldName, string OldValue, string NewValue, DateTime TransactionChangeTime)
{
    public int Id { get; set; }
    public int TransactionId { get; set; } = TransactionId;
    public FieldChange FieldName { get; set; } = FieldName;
    public string OldValue { get; set; } = OldValue;
    public string NewValue { get; set; } = NewValue;
    public DateTime TransactionChangeTime { get; set; } = TransactionChangeTime;
}

public enum FieldChange
{
    CardNumber,
    LocationCode,
    ProductName,
    Amount,
    TransactionTime,
    Status
}