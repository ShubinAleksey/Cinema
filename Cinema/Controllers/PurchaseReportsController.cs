using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PurchaseReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseReport>>> GetPurchaseReport()
        {
            if (_context.PurchaseReport == null)
            {
                return NotFound();
            }
            return await _context.PurchaseReport.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseReport>> GetPurchaseReport(long id)
        {
            if (_context.PurchaseReport == null)
            {
                return NotFound();
            }
            var purchaseReport = await _context.PurchaseReport.FindAsync(id);

            if (purchaseReport == null)
            {
                return NotFound();
            }

            return purchaseReport;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseReport(long id, PurchaseReport purchaseReport)
        {
            if (id != purchaseReport.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseReportExists(id))
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
        public async Task<ActionResult<PurchaseReport>> PostPurchaseReport(PurchaseReport purchaseReport)
        {
            if (_context.PurchaseReport == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PurchaseReport'  is null.");
            }
            _context.PurchaseReport.Add(purchaseReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseReport", new { id = purchaseReport.Id }, purchaseReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseReport(long id)
        {
            if (_context.PurchaseReport == null)
            {
                return NotFound();
            }
            var purchaseReport = await _context.PurchaseReport.FindAsync(id);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            _context.PurchaseReport.Remove(purchaseReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseReportExists(long id)
        {
            return (_context.PurchaseReport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
