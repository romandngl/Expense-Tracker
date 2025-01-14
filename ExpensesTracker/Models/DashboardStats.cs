using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesTracker.Models
{
    public class DashboardStats
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalInflows { get; set; }
        public decimal TotalOutflows { get; set; }
    }

}
