using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionIngest.Models;

public class Transaction
{
    public Transaction()
    {
        TransactionChanges = [];
    }

    public Transaction(int transactionId, string cardNumber, string locationCode, string productName, decimal amount, DateTime transactionTime, bool isRevoked)
    {
        TransactionId = transactionId;
        CardNumber = cardNumber;
        LocationCode = locationCode;
        ProductName = productName;
        Amount = amount;
        TransactionTime = transactionTime;
        IsRevoked = isRevoked;
        TransactionChanges = [];
    }

    public int TransactionId { get; set; }
    [MaxLength(20)]
    public string CardNumber { get; set; } = null!;
    [MaxLength(20)] 
    public string LocationCode { get; set; } = null!;
    [MaxLength(20)] 
    public string ProductName { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")] 
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
    public bool IsRevoked { get; set; }
    public List<TransactionChange> TransactionChanges { get; set; } = [];

    // There is definitely a better way to do this, but for the sake of time, this is what I have. :(
    public void UpdateFrom(Transaction newTransaction, DateTime now)
    {

        if (CardNumber != newTransaction.CardNumber)
        {
            TransactionChanges.Add(new TransactionChange
            (
                TransactionId,
                FieldChange.CardNumber,
                CardNumber,
                newTransaction.CardNumber,
                now
            ));

            CardNumber = newTransaction.CardNumber;
        }

        if (Amount != newTransaction.Amount)
        {
            TransactionChanges.Add(new TransactionChange(
                TransactionId,
                FieldChange.Amount,
                Amount.ToString(),
                newTransaction.Amount.ToString(),
                now
            ));

            Amount = newTransaction.Amount;
        }

        if (ProductName != newTransaction.ProductName)
        {
            TransactionChanges.Add(new TransactionChange(
                 TransactionId,
                 FieldChange.ProductName,
                 ProductName,
                 newTransaction.ProductName,
                 now
            ));

            ProductName = newTransaction.ProductName;
        }

        if (LocationCode != newTransaction.LocationCode)
        {
            TransactionChanges.Add(new TransactionChange(
                TransactionId,
                FieldChange.LocationCode,
                LocationCode,
                newTransaction.LocationCode,
                now
                ));

            LocationCode = newTransaction.LocationCode;
        }

        if (TransactionTime != newTransaction.TransactionTime)
        {
            TransactionChanges.Add(new TransactionChange(
                TransactionId,
                FieldChange.TransactionTime,
                TransactionTime.ToString(),
                newTransaction.TransactionTime.ToString(),
                now
                ));
            TransactionTime = newTransaction.TransactionTime;
        }
    }

    public void RevokeTransaction(DateTime now)
    {
        if (IsRevoked)
            return;

        IsRevoked = true;

        TransactionChanges.Add(new TransactionChange(
            TransactionId,
            FieldChange.IsRevoked,
            "False",
            "True",
            now
        ));
    }
}