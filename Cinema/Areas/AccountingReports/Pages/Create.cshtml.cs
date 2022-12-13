using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cinema.Areas.AccountingReports.Pages
{
    [Authorize(Roles = "Accountant, Administrator")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData["EmployeeId"] = new SelectList(_userManager.Users, "Id", "UserName");
            return Page();
        }

        [BindProperty]
        public AccountingReport AccountingReport { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            AccountingReport.Total = AccountingReport.Salary + AccountingReport.Bonus.GetValueOrDefault() - AccountingReport.Absence.GetValueOrDefault();

            if (!ModelState.IsValid || _context.AccountingReport == null || AccountingReport == null)
            {
                return Page();
            }

            _context.AccountingReport.Add(AccountingReport);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
