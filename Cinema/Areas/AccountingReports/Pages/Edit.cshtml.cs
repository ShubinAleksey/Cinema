using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Cinema.Areas.AccountingReports.Pages
{
    [Authorize(Roles = "Accountant, Administrator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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

            var accountingreport =  await _context.AccountingReport.FirstOrDefaultAsync(m => m.Id == id);
            if (accountingreport == null)
            {
                return NotFound();
            }
            AccountingReport = accountingreport;
            ViewData["EmployeeId"] = new SelectList(_context.Users, "Id", "UserName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AccountingReport.Total = AccountingReport.Salary + AccountingReport.Bonus.GetValueOrDefault() - AccountingReport.Absence.GetValueOrDefault();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AccountingReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountingReportExists(AccountingReport.Id))
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

        private bool AccountingReportExists(long id)
        {
          return (_context.AccountingReport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
