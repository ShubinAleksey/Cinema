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

namespace Cinema.Areas.StockReports.Pages
{
    [Authorize(Roles = "Administrator, StockManager")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

      public StockReport StockReport { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.StockReport == null)
            {
                return NotFound();
            }

            var stockreport = await _context.StockReport.FirstOrDefaultAsync(m => m.Id == id);
            if (stockreport == null)
            {
                return NotFound();
            }
            else 
            {
                StockReport = stockreport;
            }
            return Page();
        }
    }
}
