using Cinema.Common;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Areas.StockReports.Pages
{
    [Authorize(Roles = "Administrator, StockManager")]
    public class ChartModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ChartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<ChartViewModel> Datalist { get; set; } = new();

        public async Task OnGetAsync()
        {
            var reports = await _context.StockReport.ToListAsync();

            foreach (var report in reports)
            {
                Datalist.Add(new ChartViewModel
                {
                    Label = report.Name,
                    Data = report.Amount.ToString()
                });
            }
        }
    }
}
