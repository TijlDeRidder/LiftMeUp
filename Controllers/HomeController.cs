using LiftMeUp.Data;
using LiftMeUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using LiftMeUp.Data;
using LiftMeUp.Models;
using LiftMeUp.Controllers;

namespace ThatSneakerShopLaced.Controllers
{
    public class HomeController : LiftMeUpController
    {

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<LiftMeUpController> logger)
            : base(context, httpContextAccessor, logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            var stations = await _context.Station.ToListAsync();
            return View(stations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}