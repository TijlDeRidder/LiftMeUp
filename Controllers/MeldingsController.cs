using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LiftMeUp.Data;
using LiftMeUp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using LiftMeUp.Areas.Identity.Data;

namespace LiftMeUp.Controllers
{

    public class MeldingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeldingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Meldings
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index(int meldingId, int liftId, int stationId, string uitleg, string userId)
        {
            if(meldingId != 0 || liftId != 0 || stationId != 0 || uitleg != null || userId != null)
            {
                List<Melding> meldingen = _context.Melding.Where(m =>
                (m.MeldingId == meldingId || meldingId == 0)
                && (m.liftId == liftId || liftId == 0)
                && (m.stationId == stationId || stationId == 0)
                && (m.uitleg.Contains(uitleg) || string.IsNullOrEmpty(uitleg))
                && (m.UserId.Contains(userId) || string.IsNullOrEmpty(userId))
                ).ToList();
                ViewData["liftId"] = liftId;
                ViewData["meldingId"] = meldingId;
                ViewData["StationId"] = stationId;
                ViewData["uitleg"] = uitleg;
                ViewData["userId"] = userId;


                return View(meldingen);
            }
            else
            {
                var applicationDbContext = _context.Melding;
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Meldings/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Melding == null)
            {
                return NotFound();
            }

            var melding = await _context.Melding
                .FirstOrDefaultAsync(m => m.MeldingId == id);
            if (melding == null)
            {
                return NotFound();
            }

            return View(melding);
        }

        // GET: Meldings/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int? liftId)
        {
            var lifts = _context.Lift.Select(l => new SelectListItem { Value = l.liftId.ToString(), Text = l.name }).ToList();
            var stations = _context.Station.Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
            ViewData["Stations"] = stations;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            return View();
        }
        [Authorize(Roles = "Admin,User")]
        public IActionResult UserCreate(int? id)
        {
            var lift = _context.Lift.FirstOrDefault(l => l.liftId == id);
            var lifts = _context.Lift.Where(l => l.liftId == id).Select(l => new SelectListItem { Value = l.liftId.ToString(), Text = l.name }).ToList();
            var stations = _context.Station.Where(s => s.stationId == lift.stationId).Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
            ViewData["Stations"] = stations;
            ViewData["Lifts"] = lifts;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            return View();
        }

        // POST: Meldings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("MeldingId,liftId,stationId,isDeleted,uitleg,UserId")] Melding melding)
        {
            if (ModelState.IsValid)
            {
                var lift = _context.Lift.Where(l => l.liftId == melding.liftId).FirstOrDefault();
                lift.isWorking = false;
                melding.startDate = DateTime.Now;
                _context.SaveChanges();
                _context.Add(melding);
                await _context.SaveChangesAsync();
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", melding.UserId);
            return View(melding);
        }

        // GET: Meldings/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Melding == null)
            {
                return NotFound();
            }

            var melding = await _context.Melding.FindAsync(id);
            if (melding == null)
            {
                return NotFound();
            }
            var stations = _context.Station.Where(s => s.isDeleted == false).Select(s => new SelectListItem { Value = s.stationId.ToString(), Text = s.stationName }).ToList();
            var lifts = _context.Lift.Where(l=> l.isDeleted == false).Select(l => new SelectListItem { Value = l.liftId.ToString(), Text = l.name}).ToList();
            ViewData["Lifts"] = lifts;
            ViewData["Stations"] = stations;
            ViewData["liftId"] = melding.liftId;
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", melding.UserId);
            return View(melding);
        }

        // POST: Meldings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("MeldingId,liftId,stationId,startDate,isDeleted,uitleg,UserId")] Melding melding)
        {
            if (id != melding.MeldingId)
            {
                return NotFound();
            }
            var tempMelding = await _context.Melding.FindAsync(id);
            if (tempMelding.liftId != melding.liftId)
            {
                var lift = _context.Lift.Where(l => l.liftId == melding.liftId).FirstOrDefault();
                lift.isWorking = false;
                var lift2 = _context.Lift.Where(l => l.liftId == tempMelding.liftId).FirstOrDefault();
                lift2.isWorking = true;
                _context.SaveChanges();
            }
            _context.Entry(tempMelding).State = EntityState.Detached;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(melding);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeldingExists(melding.MeldingId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", melding.UserId);
            return View(melding);
        }

        // GET: Meldings/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Melding == null)
            {
                return NotFound();
            }

            var melding = await _context.Melding
                .FirstOrDefaultAsync(m => m.MeldingId == id);
            if (melding == null)
            {
                return NotFound();
            }

            return View(melding);
        }

        // POST: Meldings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Melding == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Melding'  is null.");
            }

            var melding = await _context.Melding.FindAsync(id);
            var lift = await _context.Lift.FindAsync(melding.liftId);
            lift.isWorking = true;
            if (melding != null)
            {
                melding.isDeleted = true;
                _context.Update(melding);
                await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeldingExists(int id)
        {
          return _context.Melding.Any(e => e.MeldingId == id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]

        public JsonResult GetByStationIdCreate(int stationId)
        {
            var lifts = _context.Lift.Where(l => l.stationId == stationId && l.isWorking).ToList();
            return Json(lifts);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetByStationIdEdit(int stationId)
        {
            var lifts = _context.Lift.Where(l => l.stationId == stationId).ToList();
            return Json(lifts);
        }
    }
}
