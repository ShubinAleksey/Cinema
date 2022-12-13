using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Models;

namespace Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountingReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountingReport>>> GetAccountingReport()
        {
            if (_context.AccountingReport == null)
            {
                return NotFound();
            }
            return await _context.AccountingReport.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountingReport>> GetAccountingReport(long id)
        {
            if (_context.AccountingReport == null)
            {
                return NotFound();
            }
            var accountingReport = await _context.AccountingReport.FindAsync(id);

            if (accountingReport == null)
            {
                return NotFound();
            }

            return accountingReport;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountingReport(long id, AccountingReport accountingReport)
        {
            if (id != accountingReport.Id)
            {
                return BadRequest();
            }

            _context.Entry(accountingReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountingReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AccountingReport>> PostAccountingReport(AccountingReport accountingReport)
        {
            if (_context.AccountingReport == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AccountingReport'  is null.");
            }
            _context.AccountingReport.Add(accountingReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountingReport", new { id = accountingReport.Id }, accountingReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountingReport(long id)
        {
            if (_context.AccountingReport == null)
            {
                return NotFound();
            }
            var accountingReport = await _context.AccountingReport.FindAsync(id);
            if (accountingReport == null)
            {
                return NotFound();
            }

            _context.AccountingReport.Remove(accountingReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountingReportExists(long id)
        {
            return (_context.AccountingReport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
