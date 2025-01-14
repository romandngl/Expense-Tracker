using System.Collections.Generic;
using System.Linq;

public class TransactionService
{
    private List<TransactionItems> transactions = new List<TransactionItems>();

    public List<TransactionItems> GetTransactions() => transactions;

    public void AddTransaction(TransactionItems transaction)
    {
        transactions.Add(transaction);
    }

    public List<TransactionItems> FilterTransactions(DateTime startDate, DateTime endDate, string selectedType, string searchQuery)
    {
        return transactions
            .Where(t => (string.IsNullOrEmpty(selectedType) || t.Type == selectedType)
                && (string.IsNullOrEmpty(searchQuery) || t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                && t.Date >= startDate && t.Date <= endDate)
            .OrderBy(t => t.Date)
            .ToList();
    }
}

public class TransactionItems
{
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public List<string> Tags { get; set; }
    public string Notes { get; set; }
}
