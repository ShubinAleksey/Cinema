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

namespace Cinema.Pages
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly Cinema.Data.ApplicationDbContext _context;

        public EditModel(Cinema.Data.ApplicationDbContext context)
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

            var moviesession =  await _context.MovieSession.FirstOrDefaultAsync(m => m.Id == id);
            if (moviesession == null)
            {
                return NotFound();
            }
            MovieSession = moviesession;
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

            _context.Attach(MovieSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieSessionExists(MovieSession.Id))
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

        private bool MovieSessionExists(long id)
        {
          return (_context.MovieSession?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
