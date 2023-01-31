using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiftMeUp.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LiftMeUp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiftsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LiftsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LiftsApi
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lift>>> GetLift()
        {
            return await _context.Lift.ToListAsync();
        }


        // GET: api/LiftsApi/5
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Lift>> GetLift(int id)
        {
            var lift = await _context.Lift.FindAsync(id);

            if (lift == null)
            {
                return NotFound();
            }

            return lift;
        }

        // PUT: api/LiftsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLift(int id, Lift lift)
        {
            if (id != lift.liftId)
            {
                return BadRequest();
            }

            _context.Entry(lift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiftExists(id))
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

        // POST: api/LiftsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Lift>> PostLift(Lift lift)
        {
            _context.Lift.Add(lift);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLift", new { id = lift.liftId }, lift);
        }

        // DELETE: api/LiftsApi/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLift(int id)
        {
            var lift = await _context.Lift.FindAsync(id);
            if (lift == null)
            {
                return NotFound();
            }

            _context.Lift.Remove(lift);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiftExists(int id)
        {
            return _context.Lift.Any(e => e.liftId == id);
        }

        // GET: api/LiftsApi/Working
        [Authorize(Roles = "User")]
        [HttpGet("Working")]
        public async Task<ActionResult<IEnumerable<Lift>>> GetLiftsWorking()
        {
            return await _context.Lift.Where(l => l.isWorking == true).ToListAsync();
        }

        // GET: api/LiftsApi/Broken
        [Authorize(Roles = "User")]
        [HttpGet("Broken")]
        public async Task<ActionResult<IEnumerable<Lift>>> GetLiftsNotWorking()
        {
            return await _context.Lift.Where(l => l.isWorking == false).ToListAsync();
        }
        // GET: api/LiftsApi/stationId/5
        [Authorize(Roles = "User")]
        [HttpGet("stationId/{id}")]
        public async Task<ActionResult<IEnumerable<Lift>>> GetLiftsByStationId(int id)
        {
            return await _context.Lift.Where(l => l.stationId == id).ToListAsync();
        }
    }
}
