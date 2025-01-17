using ExpensesTracker.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Threading.Tasks;

public class DashboardService
{
    private readonly TransactionService _transactionService;
    private readonly DebtService _debtService;

    public DashboardService(TransactionService transactionService, DebtService debtService)
    {
        _transactionService = transactionService;
        _debtService = debtService;
    }

    public async Task<DashboardModel> GetDashboardDataAsync(string email)
    {
        // Fetch transactions
        var totalInflows = _transactionService.GetTotalInflows(email);
        var totalOutflows = _transactionService.GetTotalOutflows(email);
        // Fetch debts
        var totalDebt = _debtService.GetTotalDebt(email);
        var clearedDebt = _debtService.GetClearedDebt(email);
        var pendingDebt = _debtService.GetPendingDebt(email);

        // Construct the dashboard model
        return new DashboardModel
        {
            TotalInflows = totalInflows,
            TotalOutflows = totalOutflows,
            TotalDebts = totalDebt,
            ClearedDebts = clearedDebt,
            RemainingDebts = totalDebt - clearedDebt,
            PendingDebts = pendingDebt.Select(debt => new DebtItems
            {
                Remarks = debt.Remarks,
                Amount = debt.Amount,
                DueDate = debt.DueDate
            }).ToList()
        };
    }
}
