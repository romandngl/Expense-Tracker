using ExpensesTracker.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class TransactionService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "TransactionDetails.json");

    // Event to notify listeners of transaction changes
    public event Action OnTransactionsChanged;

    // Load transactions from JSON
    public List<UserTransactions> LoadTransactions(string email)
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
        var transactions = LoadTransactions(email);

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
        var transactions = LoadTransactions(email);
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

    internal async Task<IEnumerable<UserTransactions>> LoadTransactionsAsync(string email)
{
    // Define the path to your JSON file
    string filePath = Path.Combine(AppContext.BaseDirectory, "TransactionDetails.json");

    // Read the JSON file asynchronously
    using FileStream stream = File.OpenRead(filePath);
    
    // Deserialize the JSON content into a list of Transaction objects
    var transactions = await JsonSerializer.DeserializeAsync<List<UserTransactions>>(stream);

    // Filter transactions by the provided email (if needed)
    var userTransactions = transactions.Where(t => t.Email == email);

    return userTransactions;
}

    internal async Task<List<TransactionItems>> GetTopTransactionsAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TransactionItems>> GetTransactionsAsync(string email)
    {
        try
        {
            return await Task.Run(() => GetUserTransactions(email));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetTransactionsAsync: {ex.Message}");
            throw; // Re-throw to propagate if necessary
        }
    }
    // Get total inflows (credits) for a user
    public decimal GetTotalInflows(string email)
    {
        var transactions = GetUserTransactions(email);
        return transactions
            .Where(t => t.Type.Equals("Credit", StringComparison.OrdinalIgnoreCase))
            .Sum(t => t.Amount);
    }

    // Get total outflows (debits) for a user
    public decimal GetTotalOutflows(string email)
    {
        var transactions = GetUserTransactions(email);
        return transactions
            .Where(t => t.Type.Equals("Debit", StringComparison.OrdinalIgnoreCase))
            .Sum(t => t.Amount);
    }


}