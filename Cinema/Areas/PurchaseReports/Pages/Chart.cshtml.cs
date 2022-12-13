using Cinema.Common;
using Cinema.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Areas.PurchaseReports.Pages
{
    [Authorize(Roles = "Administrator, PurchaseManager")]
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
            var reports = _context.PurchaseReport;
            var departments = await _context.Department.ToListAsync();

            foreach (var department in departments)
            {
                Datalist.Add(new ChartViewModel
                {
                    Label = department.Name,
                    Data = (await reports.Where(r => r.DepartmentId == department.Id).CountAsync()).ToString()
                });
            }
        }
    }
}
