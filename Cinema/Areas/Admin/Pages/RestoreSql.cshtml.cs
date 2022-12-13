using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.IO.Compression;

namespace Cinema.Areas.Admin.Pages
{
    [Authorize(Roles = "Administrator")]
    public class RestoreSqlModel : PageModel
    {
        private readonly ExportSqlOptions _options;

        public RestoreSqlModel(IOptions<ExportSqlOptions> options)
        {
            _options = options.Value;
        }

        [BindProperty]
        public IFormFile Upload { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var file = _options.RestoreFile;
            using var fileStream = new FileStream(file, FileMode.Create);
            await Upload.CopyToAsync(fileStream);

            var folder = _options.PostgreSQLFolder;
            var connString = _options.ConnString;
            var bat = _options.RestoreBatName;

            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = bat,
                Arguments = $"\"{folder}\" {connString} {file}",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            try
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();

                fileStream.Close();
                System.IO.File.Delete(file);

                return RedirectToPage("/Success");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
    }
}
