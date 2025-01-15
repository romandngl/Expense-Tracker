using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class DebtService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "UserDebts.json");

    // Load debts from JSON
    public List<DebtItems> LoadDebts()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return new List<DebtItems>();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<DebtItems>>(json) ?? new List<DebtItems>();
        }
        catch
        {
            return new List<DebtItems>();
        }
    }

    // Add a new debt
    public void AddDebt(string userEmail, DebtItems newDebt)
    {
        var debts = LoadDebts();
        debts.Add(newDebt);
        SaveDebts(debts);
    }

    // Remove a debt
    public void RemoveDebt(DebtItems debtToRemove)
    {
        var debts = LoadDebts();
        debts.RemoveAll(d => d.Title == debtToRemove.Title && d.Amount == debtToRemove.Amount);
        SaveDebts(debts);
    }

    // Save debts to JSON
    private void SaveDebts(List<DebtItems> debts)
    {
        var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
