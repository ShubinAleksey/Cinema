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

namespace Cinema.Areas.Departments.Pages
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }

        public IList<Department> Department { get; set; } = default!;

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_context.Department != null)
            {
                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";

                CurrentFilter = searchString;
                IQueryable<Department> departments = _context.Department;

                if (!string.IsNullOrEmpty(searchString))
                {
                    departments = departments.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
                }

                var orderedDepartments = sortOrder switch
                {
                    "id_desc" => departments.OrderByDescending(r => r.Id),
                    "Name" => departments.OrderBy(r => r.Name),
                    "name_desc" => departments.OrderByDescending(r => r.Name),
                    _ => departments.OrderBy(r => r.Id),
                };

                Department = await orderedDepartments.ToListAsync();
            }
        }
    }
}
