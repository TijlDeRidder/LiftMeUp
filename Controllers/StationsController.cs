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

namespace LiftMeUp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stations
        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.Station.ToListAsync());
        //}
        public  async Task<IActionResult> Index(int stationId, string stationNaam)
        {
            if(stationNaam != null || stationId != 0)
            {
                List<Station> stations = _context.Station.Where(s =>
                (s.stationName.Contains(stationNaam) || string.IsNullOrEmpty(stationNaam))
                && (s.stationId == stationId || stationId == 0)
                ).ToList();
                ViewData["stationName"] = stationNaam;
                ViewData["stationId"] = stationId;
                return View(stations);
            }
            else
            {
                    return View(await _context.Station.ToListAsync());

            }
        }

        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Station == null)
            {
                return NotFound();
            }

            var station = await _context.Station
                .FirstOrDefaultAsync(m => m.stationId == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // GET: Stations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stationId,stationName,isAccesible,hasElevator,isDeleted")] Station station)
        {
            if (ModelState.IsValid)
            {
                _context.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Station == null)
            {
                return NotFound();
            }

            var station = await _context.Station.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("stationId,stationName,isAccesible,hasElevator,isDeleted")] Station station)
        {
            if (id != station.stationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(station.isDeleted == true)
                    {
                        List<Lift> lifts = await _context.Lift.Where(l => l.stationId == station.stationId).ToListAsync();
                        lifts.ForEach(l => l.isDeleted = true);
                    }
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.stationId))
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
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Station == null)
            {
                return NotFound();
            }

            var station = await _context.Station
                .FirstOrDefaultAsync(m => m.stationId == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Station == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Station'  is null.");
            }
            var station = await _context.Station.FindAsync(id);
            if (station != null)
            {
                station.isDeleted = true;
                _context.Update(station);
                List<Lift> lifts = await _context.Lift.Where(l => l.stationId == station.stationId).ToListAsync();
                lifts.ForEach(l => l.isDeleted = true);
                await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
          return _context.Station.Any(e => e.stationId == id);
        }
    }
}
