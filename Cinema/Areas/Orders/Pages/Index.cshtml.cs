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
using Microsoft.AspNetCore.Identity;

namespace Cinema.Areas.Orders.Pages
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

        [BindProperty]
        public IList<Order> Order { get; set; } = default!;

        public string IdSort { get; set; }
        public string TicketIdSort { get; set; }
        public string MovieNameSort { get; set; }
        public string SessionTimeSort { get; set; }
        public string BuyerNameSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.Order != null)
            {
                IQueryable<Order> orders;

                if (User.IsInRole("SalesManager") || User.IsInRole("Administrator"))
                {
                    orders = _context.Order
                        .Include(o => o.Buyer)
                        .Include(o => o.Ticket)
                        .Include(o => o.Ticket.Session);
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);

                    orders = _context.Order
                        .Where(o => o.BuyerId == user.Id)
                        .Include(o => o.Ticket)
                        .Include(o => o.Ticket.Session);
                }

                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                TicketIdSort = sortOrder == "TicketId" ? "ticket_id_desc" : "TicketId";
                MovieNameSort = sortOrder == "MovieName" ? "movie_name_desc" : "MovieName";
                SessionTimeSort = sortOrder == "SessionTime" ? "session_time_desc" : "SessionTime";
                BuyerNameSort = sortOrder == "BuyerName" ? "buyer_name_desc" : "BuyerName";

                if (!string.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(s => s.Ticket.Session.MovieName.ToLower().Contains(searchString.ToLower()));
                }

                var orderedOrders = sortOrder switch
                {
                    "id_desc" => orders.OrderByDescending(r => r.Id),
                    "TicketId" => orders.OrderBy(r => r.TicketId),
                    "ticket_id_desc" => orders.OrderByDescending(r => r.TicketId),
                    "MovieName" => orders.OrderBy(r => r.Ticket.Session.MovieName),
                    "movie_name_desc" => orders.OrderByDescending(r => r.Ticket.Session.MovieName),
                    "SessionTime" => orders.OrderBy(r => r.Ticket.Session.SessionTime),
                    "session_time_desc" => orders.OrderByDescending(r => r.Ticket.Session.SessionTime),
                    "BuyerName" => orders.OrderBy(r => r.Buyer.UserName),
                    "buyer_name_desc" => orders.OrderByDescending(r => r.Buyer.UserName), 
                    _ => orders.OrderBy(r => r.Id),
                };

                Order = await orderedOrders.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostConfirm(long? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }
            var order = await _context.Order.FindAsync(id);

            if (order != null)
            {
                order.IsConfirmed = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostReject(long? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }
            var order = await _context.Order.FindAsync(id);

            if (order != null)
            {
                order.IsRejected = true;
                var ticket = await _context.Ticket.FindAsync(order.TicketId);
                ticket.IsBought = false;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
