using ExpensesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesTracker.Services
{
    public interface IDashboardService
    {
        Task<DashboardModel> GetDashboardDataAsync();
    }

}
