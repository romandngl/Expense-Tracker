using System;
using System.ComponentModel.DataAnnotations;

public class TransactionItems
{
    [Key]
    public int Id { get; set; }  // Auto-generated TransactionId

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

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    public string Category { get; set; }

    public string Notes { get; set; }

    public string Type => IsCredit ? "Credit" : (IsDebit ? "Debit" : "Unknown");
}
public class UserTransactions
{
    public string Email { get; set; }  // Unique identifier for the user
    public List<TransactionItems> Transactions { get; set; } = new List<TransactionItems>();
}