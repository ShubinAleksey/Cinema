 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Cinema.Areas.StockReports.Pages
{
    [Authorize(Roles = "Administrator, StockManager")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<StockReport> StockReport { get;set; } = default!;

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string TypeSort { get; set; }
        public string AmountSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.StockReport != null)
            {
                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                TypeSort = sortOrder == "Type" ? "type_desc" : "Type";
                AmountSort = sortOrder == "Amount" ? "amount_desc" : "Amount";

                CurrentFilter = searchString;

                IQueryable<StockReport> reports = _context.StockReport;

                if (!string.IsNullOrEmpty(searchString))
                {
                    reports = reports.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                }

                var orderedReports = sortOrder switch
                {
                    "id_desc" => reports.OrderByDescending(r => r.Id),
                    "Name" => reports.OrderBy(r => r.Name),
                    "name_desc" => reports.OrderByDescending(r => r.Name),
                    "Type" => reports.OrderBy(r => r.Type),
                    "type_desc" => reports.OrderByDescending(r => r.Type),
                    "Amount" => reports.OrderBy(r => r.Amount),
                    "amount_desc" => reports.OrderByDescending(r => r.Amount),
                    _ => reports.OrderBy(r => r.Id),
                };

                StockReport = await orderedReports.AsNoTracking().ToListAsync();
            }
        }
    }
}
