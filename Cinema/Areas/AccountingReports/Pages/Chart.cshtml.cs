using Cinema.Common;
using Cinema.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.Areas.AccountingReports.Pages
{
    public class ChartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ChartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<ChartViewModel> Datalist { get; set; } = new();

        [BindProperty]
        public List<ChartViewModel> AbsenceChart { get; set; } = new();

        [BindProperty]
        public List<ChartViewModel> BonusChart { get; set; } = new();

        public async Task OnGetAsync()
        {
            var reports = await _context.AccountingReport.ToListAsync();

            foreach (var report in reports)
            {
                Datalist.Add(new ChartViewModel { Label = report.Name, Data = report.Total.ToString() });
                AbsenceChart.Add(new ChartViewModel { Label = report.Name, Data = report.Absence.ToString() });
                BonusChart.Add(new ChartViewModel { Label = report.Name, Data = report.Bonus.ToString() });
            }

            Console.WriteLine(JsonSerializer.Serialize(Datalist));
        }
    }
}
