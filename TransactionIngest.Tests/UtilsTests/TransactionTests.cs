namespace TransactionIngest.Tests.UtilsTests;

[TestClass]
public class TransactionTests
{
    [TestMethod]
    public void NoUpdates()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("1111111111111111", existingTransaction.CardNumber);
        Assert.AreEqual("STO-01", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(109.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 1, 1), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsEmpty(existingTransaction.TransactionChanges);
    }

    [TestMethod]
    public void UpdateCardNumber()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "2222222222222222",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 2, 12),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("2222222222222222", existingTransaction.CardNumber);
        Assert.AreEqual("STO-01", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(109.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 2, 12), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsNotEmpty(existingTransaction.TransactionChanges);
        var transactionChange = existingTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.CardNumber, transactionChange.FieldName);
        Assert.AreEqual("1111111111111111", transactionChange.OldValue);
        Assert.AreEqual("2222222222222222", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateLocationCode()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-02",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 2, 12),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("1111111111111111", existingTransaction.CardNumber);
        Assert.AreEqual("STO-02", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(109.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 2, 12), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsNotEmpty(existingTransaction.TransactionChanges);
        var transactionChange = existingTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.LocationCode, transactionChange.FieldName);
        Assert.AreEqual("STO-01", transactionChange.OldValue);
        Assert.AreEqual("STO-02", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateProductName()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 50Hz Monitor",
            109.99m,
            new DateTime(2026, 2, 12),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("1111111111111111", existingTransaction.CardNumber);
        Assert.AreEqual("STO-01", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 50Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(109.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 2, 12), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsNotEmpty(existingTransaction.TransactionChanges);
        var transactionChange = existingTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.ProductName, transactionChange.FieldName);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", transactionChange.OldValue);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 50Hz Monitor", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateAmount()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            200.99m,
            new DateTime(2026, 2, 12),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("1111111111111111", existingTransaction.CardNumber);
        Assert.AreEqual("STO-01", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(200.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 2, 12), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsNotEmpty(existingTransaction.TransactionChanges);
        var transactionChange = existingTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.Amount, transactionChange.FieldName);
        Assert.AreEqual("109.99", transactionChange.OldValue);
        Assert.AreEqual("200.99", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateTransactionTime()
    {
        var existingTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );

        var newTransaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 10),
            false
            );

        existingTransaction.UpdateFrom(newTransaction, new DateTime(2026, 2, 12));

        Assert.AreEqual(1, existingTransaction.TransactionId);
        Assert.AreEqual("1111111111111111", existingTransaction.CardNumber);
        Assert.AreEqual("STO-01", existingTransaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", existingTransaction.ProductName);
        Assert.AreEqual(109.99m, existingTransaction.Amount);
        Assert.AreEqual(new DateTime(2026, 1, 10), existingTransaction.TransactionTime);
        Assert.IsFalse(existingTransaction.IsRevoked);

        Assert.IsNotEmpty(existingTransaction.TransactionChanges);
        var transactionChange = existingTransaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.TransactionTime, transactionChange.FieldName);
        Assert.AreEqual(new DateTime(2026, 1, 1).ToString(), transactionChange.OldValue);
        Assert.AreEqual(new DateTime(2026, 1, 10).ToString(), transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateRevokedFalse()
    {
        var transaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            false
            );
        
        transaction.RevokeTransaction(new DateTime(2026, 2, 12));

        Assert.AreEqual(1, transaction.TransactionId);
        Assert.AreEqual("1111111111111111", transaction.CardNumber);
        Assert.AreEqual("STO-01", transaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", transaction.ProductName);
        Assert.AreEqual(109.99m, transaction.Amount);
        Assert.AreEqual (new DateTime(2026, 1, 1), transaction.TransactionTime);
        Assert.IsTrue(transaction.IsRevoked);

        Assert.IsNotEmpty(transaction.TransactionChanges);
        var transactionChange = transaction.TransactionChanges.First();
        Assert.AreEqual(FieldChange.IsRevoked, transactionChange.FieldName);
        Assert.AreEqual("False", transactionChange.OldValue);
        Assert.AreEqual("True", transactionChange.NewValue);
        Assert.AreEqual(new DateTime(2026, 2, 12), transactionChange.TransactionChangeTime);
    }

    [TestMethod]
    public void UpdateRevokedTrue() 
    {
        var transaction = new Transaction(
            1,
            "1111111111111111",
            "STO-01",
            "S2725H 27\" IPS LED FHD 100Hz Monitor",
            109.99m,
            new DateTime(2026, 1, 1),
            true
            );

        transaction.RevokeTransaction(new DateTime(2026, 2, 12));

        Assert.AreEqual(1, transaction.TransactionId);
        Assert.AreEqual("1111111111111111", transaction.CardNumber);
        Assert.AreEqual("STO-01", transaction.LocationCode);
        Assert.AreEqual("S2725H 27\" IPS LED FHD 100Hz Monitor", transaction.ProductName);
        Assert.AreEqual(109.99m, transaction.Amount);
        Assert.AreEqual(new DateTime(2026, 1, 1), transaction.TransactionTime);
        Assert.IsTrue(transaction.IsRevoked);

        Assert.IsEmpty(transaction.TransactionChanges);
    }


}