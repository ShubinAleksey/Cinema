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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

            var purchasereport = await _context.PurchaseReport
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (purchasereport == null)
            {
                return NotFound();
            }
            else 
            {
                PurchaseReport = purchasereport;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.PurchaseReport == null)
            {
                return NotFound();
            }
            var purchasereport = await _context.PurchaseReport
                .FirstAsync(p => p.Id == id);

            if (purchasereport != null)
            {
                PurchaseReport = purchasereport;
                _context.PurchaseReport.Remove(PurchaseReport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
