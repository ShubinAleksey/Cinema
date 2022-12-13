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

namespace Cinema.Areas.PurchaseReports.Pages
{
    [Authorize(Roles = "Administrator, PurchaseManager")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string DescriptionSort { get; set; }
        public string DepartmentNameSort { get; set; }
        public string CurrentFilter { get; set; }

        public IList<PurchaseReport> PurchaseReport { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.PurchaseReport != null)
            {
                IQueryable<PurchaseReport> reports = _context.PurchaseReport.Include(s => s.Department);

                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                DescriptionSort = sortOrder == "Description" ? "description_desc" : "Description";
                DepartmentNameSort = sortOrder == "DepartmentName" ? "dep_name_desc" : "DepartmentName";

                CurrentFilter = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    reports = reports.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                }

                var orderedReports = sortOrder switch
                {
                    "id_desc" => reports.OrderByDescending(r => r.Id),
                    "Name" => reports.OrderBy(r => r.Name),
                    "name_desc" => reports.OrderByDescending(r => r.Name),
                    "Description" => reports.OrderBy(r => r.Description),
                    "description_desc" => reports.OrderByDescending(r => r.Description),
                    "DepartmentName" => reports.OrderBy(r => r.Department.Name),
                    "dep_name_desc" => reports.OrderByDescending(r => r.Department.Name),
                    _ => reports.OrderBy(r => r.Id),
                };

                PurchaseReport = await orderedReports.ToListAsync();
            }
        }
    }
}
