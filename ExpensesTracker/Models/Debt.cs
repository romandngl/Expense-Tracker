using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


public class DebtItems
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; }

    [Required(ErrorMessage = "Source is required.")]
    public string Source { get; set; }

    public string Remarks { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; }
    public bool IsCleared { get; internal set; }
}

public class UserDebts
{
    public string Email { get; set; }
    public int Id { get; set; }
    public List<DebtItems> Debts { get; set; } = new List<DebtItems>();
    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
    public bool IsCleared { get; internal set; }
    public DateTime DueDate { get; set; }
    public string Remarks { get; set; }


}