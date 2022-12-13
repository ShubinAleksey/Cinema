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
    public class StockReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockReport>>> GetStockReport()
        {
            if (_context.StockReport == null)
            {
                return NotFound();
            }
            return await _context.StockReport.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockReport>> GetStockReport(long id)
        {
            if (_context.StockReport == null)
            {
                return NotFound();
            }
            var stockReport = await _context.StockReport.FindAsync(id);

            if (stockReport == null)
            {
                return NotFound();
            }

            return stockReport;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockReport(long id, StockReport stockReport)
        {
            if (id != stockReport.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockReportExists(id))
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
        public async Task<ActionResult<StockReport>> PostStockReport(StockReport stockReport)
        {
            if (_context.StockReport == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StockReport'  is null.");
            }
            _context.StockReport.Add(stockReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockReport", new { id = stockReport.Id }, stockReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockReport(long id)
        {
            if (_context.StockReport == null)
            {
                return NotFound();
            }
            var stockReport = await _context.StockReport.FindAsync(id);
            if (stockReport == null)
            {
                return NotFound();
            }

            _context.StockReport.Remove(stockReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockReportExists(long id)
        {
            return (_context.StockReport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
