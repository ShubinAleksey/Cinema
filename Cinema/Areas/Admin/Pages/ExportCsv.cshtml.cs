using Cinema.Data;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;

namespace Cinema.Areas.Admin.Pages
{
    [Authorize(Roles = "Administrator")]
    public class ExportCsvModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ExportCsvModel(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms, System.Text.Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            writer.WriteLine("AccountingReport");
            csv.WriteRecords(await _context.AccountingReport.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("Department");
            csv.WriteRecords(await _context.Department.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("MovieSession");
            csv.WriteRecords(await _context.MovieSession.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("Order");
            csv.WriteRecords(await _context.Order.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("PurchaseReport");
            csv.WriteRecords(await _context.PurchaseReport.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("StockReport");
            csv.WriteRecords(await _context.StockReport.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("Ticket");
            csv.WriteRecords(await _context.Ticket.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("Users");
            csv.WriteRecords(await _userManager.Users.ToListAsync());
            csv.NextRecord();

            writer.WriteLine("Roles");
            csv.WriteRecords(await _roleManager.Roles.ToListAsync());
            csv.NextRecord();

            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            var result = ms.ToArray();
            ms.Close();

            return File(result, "application/force-download", "backup.csv");
        }
    }
}
