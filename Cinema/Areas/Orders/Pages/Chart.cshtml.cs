using Cinema.Common;
using Cinema.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Areas.Orders.Pages
{
    public class ChartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChartModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public List<ChartViewModel> UserChart { get; set; } = new();

        [BindProperty]
        public List<ChartViewModel> MovieChart { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var orders = _context.Order;

            foreach (var user in users)
            {
                UserChart.Add(new ChartViewModel
                {
                    Label = user.UserName,
                    Data = (await orders.Where(o => o.BuyerId == user.Id).CountAsync()).ToString()
                });
            }

            var sessions = _context.MovieSession;
            var movieNames = sessions.Select(s => s.MovieName).ToHashSet();
            var tickets = _context.Ticket;

            foreach (var name in movieNames)
            {
                MovieChart.Add(new ChartViewModel
                {
                    Label = name,
                    Data = (await tickets.Where(t => t.Session.MovieName == name && t.IsBought).CountAsync()).ToString()
                });
            }
        }
    }
}
