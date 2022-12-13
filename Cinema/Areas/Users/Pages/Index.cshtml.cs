using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Areas.Users.Pages
{
    [Authorize(Roles = "Administrator, HrManager")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IList<UserViewModel> Users { get; set; } = new List<UserViewModel>();

        public string IdSort { get; set; }
        public string NameSort { get; set; }
        public string EmailSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            if (_roleManager != null && _userManager != null)
            {
                IdSort = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                NameSort = sortOrder == "Name" ? "name_desc" : "Name";
                EmailSort = sortOrder == "Email" ? "email_desc" : "Email";

                CurrentFilter = searchString;

                var users = await _userManager.Users.ToListAsync();

                foreach (var user in users)
                {
                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Roles = string.Join(", ", await _userManager.GetRolesAsync(user))
                    };
                    Users.Add(userViewModel);
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    Users = Users
                        .Where(s => s.UserName.ToLower().Contains(searchString.ToLower()))
                        .ToList();
                }

                var orderedUsers = sortOrder switch
                {
                    "id_desc" => Users.OrderByDescending(r => r.Id),
                    "Name" => Users.OrderBy(r => r.UserName),
                    "name_desc" => Users.OrderByDescending(r => r.UserName),
                    "Email" => Users.OrderBy(r => r.Email),
                    "email_desc" => Users.OrderByDescending(r => r.Email),
                    _ => Users.OrderBy(r => r.Id),
                };

                Users = orderedUsers.ToList();
            }
        }
    }

    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Роли")]
        public string Roles { get; set; } = string.Empty;
    }
}
