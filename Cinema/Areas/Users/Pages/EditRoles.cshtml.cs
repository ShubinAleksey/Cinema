using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Areas.Users.Pages
{
    [Authorize(Roles = "Administrator, HrManager")]
    public class EditRolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public EditRolesModel(
             RoleManager<IdentityRole> roleManager,
             UserManager<IdentityUser> userManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IList<ManageUserRolesViewModel> UserRoles = new List<ManageUserRolesViewModel>();
        public string UserID { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            UserID = id;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Page();
            }

            UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles.ToList())
            {
                ManageUserRolesViewModel roles = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };
                UserRoles.Add(roles);

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roles.Selected = true;
                }
                else
                {
                    roles.Selected = false;
                }
                model.Add(roles);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<ManageUserRolesViewModel> model, string id)
        {
            UserID = id;

            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Page();
                }

                UserName = user.UserName;
                var roles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return Page();
                }

                var selectedRoles = model.Where(x => x.Selected).Select(y => y.RoleName);

                await _userManager.AddToRolesAsync(user, selectedRoles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return Page();
                }

                return RedirectToPage("./Index");
            }

            return Page();
        }

        public class ManageUserRolesViewModel
        {
            public string RoleId { get; set; } = string.Empty;
            public string RoleName { get; set; } = string.Empty;
            public bool Selected { get; set; }
        }
    }
}
