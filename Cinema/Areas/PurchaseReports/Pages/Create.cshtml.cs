using Cinema.Common;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Areas.PurchaseReports.Pages
{
    [Authorize(Roles = "Administrator, PurchaseManager")]
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public PurchaseReport PurchaseReport { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyReport = new PurchaseReport();

            if (await TryUpdateModelAsync<PurchaseReport>(
                 emptyReport,
                 "PurchaseReport",
                 r => r.Id, r => r.Name, r => r.Description, r => r.DepartmentId))
            {
                _context.PurchaseReport.Add(emptyReport);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentsDropDownList(_context, emptyReport.DepartmentId);
            return Page();
        }
    }
}
