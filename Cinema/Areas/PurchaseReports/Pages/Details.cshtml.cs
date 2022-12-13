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
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public PurchaseReport PurchaseReport { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.PurchaseReport == null)
            {
                return NotFound();
            }

            var purchasereport = await _context.PurchaseReport
                .Include(p => p.Department)
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
    }
}
