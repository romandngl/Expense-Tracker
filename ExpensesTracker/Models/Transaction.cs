using System;
using System.Collections.Generic;

public class TransactionItems
{
    private bool _isDebit;
    private bool _isCredit;

    public bool IsDebit
    {
        get => _isDebit;
        set
        {
            _isDebit = value;
            if (_isDebit) IsCredit = false; // Automatically uncheck Credit
        }
    }

    public bool IsCredit
    {
        get => _isCredit;
        set
        {
            _isCredit = value;
            if (_isCredit) IsDebit = false; // Automatically uncheck Debit
        }
    }

    public string Title { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
    public string Notes { get; set; }
    public string Type => IsCredit ? "Credit" : (IsDebit ? "Debit" : "Unknown");

}

