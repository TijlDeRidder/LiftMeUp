using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiftMeUp.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static System.Collections.Specialized.BitVector32;

namespace LiftMeUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LiftsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LiftsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lifts
        public async Task<IActionResult> Index(int liftId, string liftNaam, int stationId)
        {
            if (liftNaam != null || liftId != 0 || stationId != 0)
            {
                List<Lift> lifts = _context.Lift.Where(l =>
                (l.name.Contains(liftNaam) || string.IsNullOrEmpty(liftNaam))
                && (l.liftId == liftId || liftId == 0)
                && (l.stationId == stationId || stationId == 0)
                ).ToList();

                return View(lifts);
            }
            else
            {
                return View(await _context.Lift.ToListAsync());
            }
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
            var stations = _context.Station.Where(s => s.isDeleted == false).Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
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
            var stations = _context.Station.Where(s => s.isDeleted == false).Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
            ViewData["Stations"] = stations;
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
                    if (lift.isWorking)
                    {
                        Notification notification = new Notification()
                        {
                            
                            createTime = DateTime.Now,
                            isFixed = true,
                            liftName = lift.name,
                            Lift = lift
                        };
                        _context.Notification.Add(notification);
                    }
                    else
                    {
                        Notification notification = new Notification()
                        {
                            createTime = DateTime.Now,
                            isFixed = false,
                            liftName = lift.name,
                            Lift = lift
                        };
                        _context.Notification.Add(notification);
                    }
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
            _context.SaveChanges();
            var lift = await _context.Lift.FindAsync(id);
            if (lift != null)
            {
                if(lift.isWorking == false)
                {
                    var melding = await _context.Melding.Where(m => m.liftId == id).FirstAsync();
                    melding.isDeleted = true;
                }
                lift.isDeleted = true;
                _context.Update(lift);
                await _context.SaveChangesAsync();
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
