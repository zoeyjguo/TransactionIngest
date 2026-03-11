namespace TransactionIngest.Models;

public class TransactionChange
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public FieldChange FieldName { get; set; }
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime TransactionChangeTime { get; set; }
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