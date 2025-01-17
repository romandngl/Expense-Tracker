using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesTracker.Models
{
    public class DashboardModel
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalInflows { get; set; }
        public decimal TotalOutflows { get; set; }
        public decimal TotalDebts { get; set; }
        public decimal ClearedDebts { get; set; }
        public decimal RemainingDebts { get; set; }
        public List<DebtItems> PendingDebts { get; set; } = new();

        public List<TransactionItems> TopTransactions { get; set; }

        public DashboardModel()
        {
            TotalAmount = 0;
            TotalInflows = 0;
            TotalOutflows = 0;
            TotalDebts = 0;
            ClearedDebts = 0;
            RemainingDebts = 0;
            TopTransactions = new List<TransactionItems>();
            PendingDebts = new List<DebtItems>();
        }
    }

}
