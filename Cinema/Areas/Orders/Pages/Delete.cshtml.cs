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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FirstOrDefaultAsync(m => m.Id == id);
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (
                order == null || currentUser == null 
                || (order.BuyerId != currentUser.Id && !User.IsInRole("Administrator") && !User.IsInRole("SalesManager"))
            )
            {
                return NotFound();
            }
            else
            {
                Order = order;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (
                order != null && currentUser != null 
                && (order.BuyerId == currentUser.Id || User.IsInRole("Administrator") || User.IsInRole("SalesManager"))
            )
            {
                Order = order;
                var ticket = await _context.Ticket.FindAsync(Order.TicketId);

                if (ticket != null)
                {
                    ticket.IsBought = false;
                }

                _context.Order.Remove(Order);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
