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

namespace Cinema.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string TimeSort { get; set; }
        public string CurrentFilter { get; set; }

        public IList<MovieSession> MovieSession { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.MovieSession != null)
            {
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                TimeSort = sortOrder == "Time" ? "time_desc" : "Time";

                CurrentFilter = searchString;

                IQueryable<MovieSession> sessions = _context.MovieSession
                    .Where(s => s.SessionTime > DateTime.UtcNow);

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
                    _ => sessions.OrderBy(r => r.MovieName),
                };

                MovieSession = await orderedSessions
                    .ToListAsync();
            }
        }
    }
}
