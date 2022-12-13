using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Pages
{
    [Authorize(Roles = "Administrator")]
    public class OutdatedModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OutdatedModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MovieSession> OutdatedSession { get; set; } = default!;

        public string NameSort { get; set; }
        public string TimeSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.MovieSession != null)
            {
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                TimeSort = sortOrder == "Time" ? "time_desc" : "Time";

                CurrentFilter = searchString;

                IQueryable<MovieSession> sessions = _context.MovieSession
                    .Where(s => s.SessionTime <= DateTime.UtcNow);

                if (!string.IsNullOrEmpty(searchString))
                {
                    sessions = sessions.Where(s => s.MovieName.ToLower().Contains(searchString.ToLower()));
                }

                var orderedSessions = sortOrder switch
                {
                    "Name" => sessions.OrderBy(r => r.MovieName),
                    "name_desc" => sessions.OrderByDescending(r => r.MovieName),
                    "Time" => sessions.OrderBy(r => r.SessionTime),
                    "time_desc" => sessions.OrderByDescending(r => r.SessionTime),
                    _ => sessions.OrderByDescending(r => r.SessionTime),
                };

                OutdatedSession = await orderedSessions.AsNoTracking().ToListAsync();
            }
        }
    }
}
