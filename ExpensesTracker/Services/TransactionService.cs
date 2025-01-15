using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class TransactionService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "TransactionDetails.json");

    // Load transactions from JSON
    public List<UserTransactions> LoadTransactions()
    {
        if (!File.Exists(FilePath))
        {
            return new List<UserTransactions>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<UserTransactions>>(json) ?? new List<UserTransactions>();
    }

    // Save transactions to JSON
    public void SaveTransactions(List<UserTransactions> transactions)
    {
        var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    // Add a new transaction for a user
    public void AddTransaction(string email, TransactionItems newTransaction)
    {
        var transactions = LoadTransactions();

        // Find existing user transactions or create a new entry
        var userTransaction = transactions.Find(ut => ut.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (userTransaction == null)
        {
            userTransaction = new UserTransactions { Email = email, Transactions = new List<TransactionItems>() };
            transactions.Add(userTransaction);
        }

        // Auto-generate TransactionId
        newTransaction.Id = userTransaction.Transactions.Count > 0
            ? userTransaction.Transactions.Max(t => t.Id) + 1
            : 1;

        userTransaction.Transactions.Add(newTransaction);
        SaveTransactions(transactions);
    }

    // Get all transactions for a user by email
    public List<TransactionItems> GetUserTransactions(string email)
    {
        var transactions = LoadTransactions();
        var userTransaction = transactions.Find(ut => ut.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return userTransaction?.Transactions ?? new List<TransactionItems>();
    }

    // Filter transactions by date and type
    public List<TransactionItems> FilterTransactions(string email, DateTime startDate, DateTime endDate, string type = null)
    {
        var userTransactions = GetUserTransactions(email);

        return userTransactions
            .Where(t => t.Date >= startDate && t.Date <= endDate &&
                        (string.IsNullOrEmpty(type) || t.Type.Equals(type, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
