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

namespace Cinema.Areas.AccountingReports.Pages
{
    [Authorize(Roles = "Accountant, Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountingReport AccountingReport { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.AccountingReport == null)
            {
                return NotFound();
            }

            var accountingreport = await _context.AccountingReport
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (accountingreport == null)
            {
                return NotFound();
            }
            else 
            {
                AccountingReport = accountingreport;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.AccountingReport == null)
            {
                return NotFound();
            }
            var accountingreport = await _context.AccountingReport.FindAsync(id);

            if (accountingreport != null)
            {
                AccountingReport = accountingreport;
                _context.AccountingReport.Remove(AccountingReport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
