using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Areas.Roles.Pages
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _roleManager == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            
            if (role == null)
            {
                return NotFound();
            }
            
            Role = role;
            return Page();
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _roleManager.UpdateAsync(Role);
            }
            catch (Exception) 
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
