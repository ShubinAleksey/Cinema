using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Cinema.Areas.Admin.Pages
{
    [Authorize(Roles = "Administrator")]
    public class CleanModel : PageModel
    {
        private readonly ExportSqlOptions _options;

        public CleanModel(IOptions<ExportSqlOptions> options)
        {
            _options = options.Value;
        }
        public IActionResult OnGet()
        {
            var file = _options.ResultFile;
            try
            {
                System.IO.File.Delete(file);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToPage("./Index");
            }
        }
    }
}
