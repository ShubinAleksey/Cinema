using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cinema.Data;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Cinema.Areas.Session.Pages
{
    [Authorize(Roles = "Administrator")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["SessionId"] = new SelectList(new[] { _context.MovieSession.First(s => s.Id == Id) }, 
                "Id", "MovieName");

            return Page();
        }

        [BindProperty]
        public Ticket Ticket { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var emptyTicket = new Ticket();

            if (await TryUpdateModelAsync<Ticket>(
                 emptyTicket,
                 "Ticket",
                 t => t.Id, t => t.SessionId, t => t.RowNumber, t => t.SeatNumber, t => t.IsBought))
            {
                _context.Ticket.Add(emptyTicket);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index", new { id = Id });
            }

            return Page();
        }
    }
}
