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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PurchaseReport PurchaseReport { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.PurchaseReport == null)
            {
                return NotFound();
            }

            var purchasesreport =  await _context.PurchaseReport.FirstOrDefaultAsync(m => m.Id == id);
            if (purchasesreport == null)
            {
                return NotFound();
            }
            PurchaseReport = purchasesreport;
           ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PurchaseReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesReportExists(PurchaseReport.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SalesReportExists(long id)
        {
          return (_context.PurchaseReport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
