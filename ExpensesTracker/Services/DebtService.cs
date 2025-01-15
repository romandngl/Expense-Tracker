using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class DebtService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "UserDebts.json");

    // Load debts from JSON
    public List<UserDebts> LoadDebts()
    {
        if (!File.Exists(FilePath))
        {
            return new List<UserDebts>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<UserDebts>>(json) ?? new List<UserDebts>();
    }

    // Save debts to JSON
    public void SaveDebts(List<UserDebts> debts)
    {
        var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    // Add a new debt for a user
    public void AddDebt(string email, DebtItems newDebt)
    {
        var debts = LoadDebts();

        // Find existing user debts or create a new entry
        var userDebts = debts.Find(ud => ud.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (userDebts == null)
        {
            userDebts = new UserDebts { Email = email };
            debts.Add(userDebts);
        }

        // Auto-generate DebtId
        newDebt.Id = userDebts.Debts.Count > 0
            ? userDebts.Debts.Max(d => d.Id) + 1
            : 1;

        userDebts.Debts.Add(newDebt);
        SaveDebts(debts);
    }

    // Get all debts for a user by email
    public List<DebtItems> GetUserDebts(string email)
    {
        var debts = LoadDebts();
        var userDebts = debts.Find(ud => ud.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return userDebts?.Debts ?? new List<DebtItems>();
    }

    // Update a specific debt
    public void UpdateDebt(string email, DebtItems updatedDebt)
    {
        var debts = LoadDebts();

        var userDebts = debts.Find(ud => ud.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (userDebts != null)
        {
            var debt = userDebts.Debts.Find(d => d.Id == updatedDebt.Id);
            if (debt != null)
            {
                debt.Title = updatedDebt.Title;
                debt.Amount = updatedDebt.Amount;
                debt.Category = updatedDebt.Category;
                debt.Source = updatedDebt.Source;
                debt.Remarks = updatedDebt.Remarks;
                debt.Date = updatedDebt.Date;
                debt.DueDate = updatedDebt.DueDate;
                SaveDebts(debts);
            }
        }
    }

    // Delete a debt
    public void DeleteDebt(string email, int debtId)
    {
        var debts = LoadDebts();

        var userDebts = debts.Find(ud => ud.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (userDebts != null)
        {
            userDebts.Debts.RemoveAll(d => d.Id == debtId);
            SaveDebts(debts);
        }
    }

    // Filter debts by date range and category
    public List<DebtItems> FilterDebts(string email, DateTime? startDate, DateTime? endDate, string category = null)
    {
        var userDebts = GetUserDebts(email);

        return userDebts
            .Where(d =>
                (!startDate.HasValue || d.Date >= startDate.Value) &&
                (!endDate.HasValue || d.Date <= endDate.Value) &&
                (string.IsNullOrEmpty(category) || d.Category.Equals(category, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    // Sort debts by date (latest or oldest)
    public List<DebtItems> SortDebtsByDate(List<DebtItems> debts, bool sortByLatest = true)
    {
        return sortByLatest
            ? debts.OrderByDescending(d => d.Date).ToList()
            : debts.OrderBy(d => d.Date).ToList();
    }

    // Search debts by title
    public List<DebtItems> SearchDebtsByTitle(string email, string searchTerm)
    {
        var userDebts = GetUserDebts(email);

        if (string.IsNullOrEmpty(searchTerm))
        {
            return userDebts;
        }

        return userDebts
            .Where(d => d.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
