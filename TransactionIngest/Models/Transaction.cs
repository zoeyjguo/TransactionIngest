using System;
using System.Collections.Generic;
using System.Text;
using TransactionIngest.Services;

namespace TransactionIngest.Models;

public class Transaction(int transactionId, string cardNumber, string locationCode, string productName, decimal amount, DateTime transactionTime, Boolean isRevoked)
{
    public int TransactionId { get; protected set; } = transactionId;
    public string CardNumber { get; protected set; } = cardNumber;
    public string LocationCode { get; protected set; } = locationCode;
    public string ProductName { get; protected set; } = productName;
    public decimal Amount { get; protected set; } = amount;
    public DateTime TransactionTime { get; protected set; } = transactionTime;
    public Boolean isRevoked { get; protected set; } = isRevoked;
    public List<TransactionChange> TransactionChanges { get; protected set; } = new List<TransactionChange>();

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
}

public enum Status
{
    Active,
    Revoked,
    Finalized
}