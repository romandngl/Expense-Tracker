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

    // Filter transactions by date range, type, and category
    public List<TransactionItems> FilterTransactions(string email, DateTime? startDate, DateTime? endDate, string type = null, string category = null)
    {
        var userTransactions = GetUserTransactions(email);

        // Apply filters
        var filteredTransactions = userTransactions.AsQueryable();

        if (startDate.HasValue)
        {
            filteredTransactions = filteredTransactions.Where(t => t.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            filteredTransactions = filteredTransactions.Where(t => t.Date <= endDate.Value);
        }

        if (!string.IsNullOrEmpty(type) && type != "all")
        {
            filteredTransactions = filteredTransactions.Where(t => t.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(category) && category != "all")
        {
            filteredTransactions = filteredTransactions
                .Where(t => t.Category != null && t.Category.Contains(category, StringComparison.OrdinalIgnoreCase));
        }

        return filteredTransactions.ToList();
    }

    // Sort transactions by date (latest or oldest)
    public List<TransactionItems> SortTransactionsByDate(List<TransactionItems> transactions, bool sortByLatest = true)
    {
        return sortByLatest
            ? transactions.OrderByDescending(t => t.Date).ToList()
            : transactions.OrderBy(t => t.Date).ToList();
    }

    // Search transactions by title
    public List<TransactionItems> SearchTransactionsByTitle(string email, string searchTerm)
    {
        var userTransactions = GetUserTransactions(email);

        if (string.IsNullOrEmpty(searchTerm))
        {
            return userTransactions;
        }

        return userTransactions
            .Where(t => t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // Get all unique categories for a user's transactions
    public List<string> GetTransactionCategories(string email)
    {
        var userTransactions = GetUserTransactions(email);

        // Extract unique categories from all transactions
        return userTransactions
            .Where(t => !string.IsNullOrEmpty(t.Category)) // Ensure category is not null or empty
            .Select(t => t.Category)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
