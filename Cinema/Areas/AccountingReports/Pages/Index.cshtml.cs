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

namespace Cinema.Areas.AccountingReports.Pages
{
    [Authorize(Roles = "Accountant, Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string EmployeeNameSort { get; set; }
        public string TotalSort { get; set; }
        public string CurrentFilter { get; set; }

        public IList<AccountingReport> AccountingReport { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.AccountingReport != null)
            {
                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                EmployeeNameSort = sortOrder == "EmployeeName" ? "empl_name_desc" : "EmployeeName";
                TotalSort = sortOrder == "Total" ? "total_desc" : "Total";

                CurrentFilter = searchString;

                IQueryable<AccountingReport> reports = _context.AccountingReport.Include(a => a.Employee);

                if (!string.IsNullOrEmpty(searchString))
                {
                    reports = reports.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                }

                var orderedReports = sortOrder switch
                {
                    "id_desc" => reports.OrderByDescending(r => r.Id),
                    "Name" => reports.OrderBy(r => r.Name),
                    "name_desc" => reports.OrderByDescending(r => r.Name),
                    "EmployeeName" => reports.OrderBy(r => r.Employee.UserName),
                    "empl_name_desc" => reports.OrderByDescending(r => r.Employee.UserName),
                    "Total" => reports.OrderBy(r => r.Total),
                    "total_desc" => reports.OrderByDescending(r => r.Total),
                    _ => reports.OrderBy(r => r.Id),
                };
                
                AccountingReport = await orderedReports.AsNoTracking().ToListAsync();
            }
        }
    }
}
