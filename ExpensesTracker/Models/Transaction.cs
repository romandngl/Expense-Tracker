using System;
using System.Collections.Generic;

public class TransactionItems
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string Notes { get; set; }
    public bool IsDebit { get; set; }
    public bool IsCredit { get; set; }

    // Computed property for transaction type
    public string Type => IsDebit ? "Debit" : IsCredit ? "Credit" : "Unknown";
}
