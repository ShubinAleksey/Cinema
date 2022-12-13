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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Areas.Session.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Ticket> Tickets { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public Ticket SelectedTicket { get; set; } = default!;

        public MovieSession Session { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Ticket != null)
            {
                Session = await _context.MovieSession.FirstAsync(s => s.Id == Id);

                Tickets = await _context.Ticket.Where(t => t.SessionId == Id)
                .Include(t => t.Session).ToListAsync();

                ViewData["TicketId"] = new SelectList(Tickets
                    .Where(t => !t.IsBought)
                    .Select(t => new
                    {
                        t.Id,
                        Text = $"Ряд {t.RowNumber}, место {t.SeatNumber}"
                    }),
                    "Id",
                    "Text");
            }
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Ticket == null || SelectedTicket == null)
            {
                return Page();
            }

            var ticket = await _context.Ticket.FirstAsync(s => s.Id == SelectedTicket.Id);
            ticket.IsBought = true;
            _context.Attach(ticket).State = EntityState.Modified;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var order = new Order
            {
                BuyerId = user.Id,
                TicketId = SelectedTicket.Id,
                IsConfirmed = false
            };

            _context.Order.Add(order);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Success");
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}
