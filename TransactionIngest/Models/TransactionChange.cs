using System.Text.Json.Serialization;
using TransactionIngest.JsonConverter;

namespace TransactionIngest.Models;

public class TransactionChange
{
    public TransactionChange() { }

    public TransactionChange(int transactionId, FieldChange fieldName, string oldValue, string newValue, DateTime transactionChangeTime)
    {
        TransactionId = transactionId;
        FieldName = fieldName;
        OldValue = oldValue;
        NewValue = newValue;
        TransactionChangeTime = transactionChangeTime;
    }

    public int Id { get; set; }
    public int TransactionId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FieldChange FieldName { get; set; }
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string OldValue { get; set; } = null!;
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string NewValue { get; set; } = null!;
    public DateTime TransactionChangeTime { get; set; }
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