using System;
using System.Collections.Generic;
using System.Linq;

public class TransactionService
{
    private List<TransactionItems> transactions = new List<TransactionItems>();

    // Method to return all transactions
    public List<TransactionItems> GetTransactions() => transactions;

    // Method to add a transaction
    public void AddTransaction(TransactionItems transaction)
    {
        transactions.Add(transaction);
    }

    // Filter transactions based on given criteria
    public List<TransactionItems> FilterTransactions(DateTime startDate, DateTime endDate, string selectedType, string searchQuery)
    {
        return transactions
            .Where(t => (string.IsNullOrEmpty(selectedType) || t.Type == selectedType)  // Type filter
                && (string.IsNullOrEmpty(searchQuery) || t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) // Search filter
                && t.Date >= startDate && t.Date <= endDate) // Date filter
            .OrderBy(t => t.Date)
            .ToList();
    }
}
