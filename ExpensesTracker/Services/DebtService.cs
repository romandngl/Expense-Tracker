using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class DebtService
{
    private readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "UserDebts.json");

    // Load debts for a specific user
    public List<DebtItems> LoadDebts(string userEmail)
    {
        if (!File.Exists(FilePath))
        {
            return new List<DebtItems>();
        }

        var allDebts = JsonSerializer.Deserialize<List<UserDebts>>(File.ReadAllText(FilePath)) ?? new List<UserDebts>();
        var userDebts = allDebts.FirstOrDefault(d => d.Email == userEmail);

        return userDebts?.Debts ?? new List<DebtItems>();
    }

    // Save debts for a specific user
    public void SaveDebts(string userEmail, List<DebtItems> debts)
    {
        List<UserDebts> allDebts;

        if (File.Exists(FilePath))
        {
            allDebts = JsonSerializer.Deserialize<List<UserDebts>>(File.ReadAllText(FilePath)) ?? new List<UserDebts>();
        }
        else
        {
            allDebts = new List<UserDebts>();
        }

        var userDebts = allDebts.FirstOrDefault(d => d.Email == userEmail);

        if (userDebts == null)
        {
            userDebts = new UserDebts { Email = userEmail, Debts = debts };
            allDebts.Add(userDebts);
        }
        else
        {
            userDebts.Debts = debts;
        }

        File.WriteAllText(FilePath, JsonSerializer.Serialize(allDebts, new JsonSerializerOptions { WriteIndented = true }));
    }

    // Add a new debt for the user
    public void AddDebt(string userEmail, DebtItems debt)
    {
        var debts = LoadDebts(userEmail);
        debts.Add(debt);
        SaveDebts(userEmail, debts);
    }

    // Remove a debt for the user
    public void RemoveDebt(string userEmail, DebtItems debt)
    {
        var debts = LoadDebts(userEmail);
        var debtToRemove = debts.FirstOrDefault(d => d.Id == debt.Id);

        if (debtToRemove != null)
        {
            debts.Remove(debtToRemove);
            SaveDebts(userEmail, debts);
        }
    }

    // Get the total debt for a user
    public decimal GetTotalDebt(string Email)
    {
        var debts = LoadDebts(Email);
        return debts.Sum(d => d.Amount);
    }

    // Get the total cleared debt for a user
    public decimal GetClearedDebt(string Email)
    {
        var debts = LoadDebts(Email);
        return debts.Where(d => d.IsCleared).Sum(d => d.Amount);
    }

    // Get the total pending debt for a user
    public List<DebtItems> GetPendingDebt(string email)
    {
        var debts = LoadDebts(email);
        return debts.Where(debt => !debt.IsCleared).ToList();
    }

    internal async Task<IEnumerable<UserDebts>> LoadDebtsAsync(string email)
    {
        // Define the path to your JSON file
        string filePath = Path.Combine(AppContext.BaseDirectory, "DebtDetails.json");

        // Read the JSON file asynchronously
        using FileStream stream = File.OpenRead(filePath);

        // Deserialize the JSON content into a list of Debt objects
        var debts = await JsonSerializer.DeserializeAsync<List<UserDebts>>(stream);

        // Filter debts by the provided email (if needed)
        var userDebts = debts.Where(d => d.Email == email);

        return userDebts;
    }

    
}