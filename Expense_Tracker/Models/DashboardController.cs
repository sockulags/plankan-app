using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Models
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {

            //Senaste 30 dagarnas transaktioner
            DateTime StartDate = DateTime.Today.AddDays(-30);
            DateTime EndDate = DateTime.Today;

            List<Transaction> SelectedTransactions = await _context.Transactions
              .Include(x=>x.Category)
              //.Where(y=>y.Date >= StartDate && y.Date <= EndDate)
              .ToListAsync();


            //Total inkomst
            int totalIncome = SelectedTransactions
                .Where(x => x.Category.Type == "Income")
                .Sum(s => s.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("C0");

            //Total utgifter
            int totalExpense = SelectedTransactions
                .Where(x => x.Category.Type == "Expense")
                .Sum(s => s.Amount);
            ViewBag.TotalExpense = totalExpense.ToString("C0");

            //Saldo
            int balance = totalIncome - totalExpense;
            //CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            //balance.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = balance.ToString("C0");

            //Doughnut chart - Utigfter efter kategori
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(x => x.Category.CategoryId)
                .Select(x => new
                {
                    CategoryTitleWithIcon = x.First().Category.Icon + " " + x.First().Category.Title,
                    amount = x.Sum(s => s.Amount),
                    formattedAmout = x.Sum(s => s.Amount).ToString("C0")
                })
                .OrderByDescending(x => x.amount)
                .ToList();

            //Spline Chart - Income vs Expense

            //Income
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(x => x.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(s=>s.Amount)
                })
                .ToList();

            //Expense
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(x => x.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(s => s.Amount)
                })
                .ToList();

            //Combine Income & Expense
            string[] last30days = Enumerable.Range(0, 30)
                .Select(i=> StartDate.AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in last30days
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into dayExpenseJoined
                                      from expense in dayExpenseJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,

                                      };

            //Senaste transaktionerna
            ViewBag.RecentTransactions = await _context.Transactions
                .Include(i => i.Category)
                .OrderByDescending(x => x.Date)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }

    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
