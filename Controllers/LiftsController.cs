using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiftMeUp.Data;
using LiftMeUp.Models;

namespace LiftMeUp.Controllers
{
    public class LiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lifts
        public async Task<IActionResult> Index()
        {
              return View(await _context.Lift.ToListAsync());
        }

        // GET: Lifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lift == null)
            {
                return NotFound();
            }

            var lift = await _context.Lift
                .FirstOrDefaultAsync(m => m.liftId == id);
            if (lift == null)
            {
                return NotFound();
            }

            return View(lift);
        }

        // GET: Lifts/Create
        public IActionResult Create()
        {
            var stations = _context.Station.Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
            ViewData["Stations"] = stations;
            return View();
        }

        // POST: Lifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("liftId,name,stationId,isWorking,isDeleted")] Lift lift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lift);
        }

        // GET: Lifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lift == null)
            {
                return NotFound();
            }

            var lift = await _context.Lift.FindAsync(id);
            if (lift == null)
            {
                return NotFound();
            }
            return View(lift);
        }

        // POST: Lifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("liftId,name,stationId,isWorking,isDeleted")] Lift lift)
        {
            if (id != lift.liftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LiftExists(lift.liftId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lift);
        }

        // GET: Lifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lift == null)
            {
                return NotFound();
            }

            var lift = await _context.Lift
                .FirstOrDefaultAsync(m => m.liftId == id);
            if (lift == null)
            {
                return NotFound();
            }

            return View(lift);
        }

        // POST: Lifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lift == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lift'  is null.");
            }
            var lift = await _context.Lift.FindAsync(id);
            if (lift != null)
            {
                _context.Lift.Remove(lift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LiftExists(int id)
        {
          return _context.Lift.Any(e => e.liftId == id);
        }
    }
}
