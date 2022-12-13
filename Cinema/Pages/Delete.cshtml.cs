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

namespace Cinema.Pages
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly Cinema.Data.ApplicationDbContext _context;

        public DeleteModel(Cinema.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public MovieSession MovieSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.MovieSession == null)
            {
                return NotFound();
            }

            var moviesession = await _context.MovieSession.FirstOrDefaultAsync(m => m.Id == id);

            if (moviesession == null)
            {
                return NotFound();
            }
            else 
            {
                MovieSession = moviesession;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.MovieSession == null)
            {
                return NotFound();
            }
            var moviesession = await _context.MovieSession.FindAsync(id);

            if (moviesession != null)
            {
                MovieSession = moviesession;
                _context.MovieSession.Remove(MovieSession);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
