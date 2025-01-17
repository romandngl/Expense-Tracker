using System;
using System.ComponentModel.DataAnnotations;

public class TransactionItems
{
    [Key]
    public int Id { get; set; }  // Auto-generated TransactionId

    private bool _isDebit;
    private bool _isCredit;

    

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    public string Category { get; set; }

    public string Notes { get; set; }

    private string _type;
    public string Type
    {
        get => IsCredit ? "Credit" : (IsDebit ? "Debit" : _type ?? "Unknown");
        set
        {
            _type = value;

            // Optionally update IsCredit and IsDebit based on the set value
            IsCredit = value.Equals("Credit", StringComparison.OrdinalIgnoreCase);
            IsDebit = value.Equals("Debit", StringComparison.OrdinalIgnoreCase);
        }
    }

    public bool IsCredit { get; set; }
    public bool IsDebit { get; set; }
    public string UserEmail { get; internal set; }
}
public class UserTransactions
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }  // Unique identifier for the user
    public List<TransactionItems> Transactions { get; set; } = new List<TransactionItems>();
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

}