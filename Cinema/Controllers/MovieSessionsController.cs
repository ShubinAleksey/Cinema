using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieSessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovieSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieSession>>> GetMovieSession()
        {
            if (_context.MovieSession == null)
            {
                return NotFound();
            }
            return await _context.MovieSession.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieSession>> GetMovieSession(long id)
        {
            if (_context.MovieSession == null)
            {
                return NotFound();
            }
            var movieSession = await _context.MovieSession.FindAsync(id);

            if (movieSession == null)
            {
                return NotFound();
            }

            return movieSession;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieSession(long id, MovieSession movieSession)
        {
            if (id != movieSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieSessionExists(id))
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
        public async Task<ActionResult<MovieSession>> PostMovieSession(MovieSession movieSession)
        {
            if (_context.MovieSession == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MovieSession'  is null.");
            }
            _context.MovieSession.Add(movieSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieSession", new { id = movieSession.Id }, movieSession);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieSession(long id)
        {
            if (_context.MovieSession == null)
            {
                return NotFound();
            }
            var movieSession = await _context.MovieSession.FindAsync(id);
            if (movieSession == null)
            {
                return NotFound();
            }

            _context.MovieSession.Remove(movieSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieSessionExists(long id)
        {
            return (_context.MovieSession?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
