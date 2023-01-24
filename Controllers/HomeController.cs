using LiftMeUp.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using LiftMeUp.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LiftMeUp.Controllers
{
    public class HomeController : LiftMeUpController
    {

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<LiftMeUpController> logger)
            : base(context, httpContextAccessor, logger)
        {
        }

        public async Task<IActionResult> Index(string LiftName)
        {
            List<Lift> lifts = _context.Lift.Where(l => l.name.Contains(LiftName) || string.IsNullOrEmpty(LiftName) && l.isDeleted == false).ToList();
            List<Melding> meldingen = _context.Melding.ToList();
            ViewBag.Meldingen = meldingen;
            return View(lifts);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Meldingen()
        {
            List<Melding> meldingen = _context.Melding.Where(m => m.isDeleted == false).ToList();
            return View(meldingen);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}