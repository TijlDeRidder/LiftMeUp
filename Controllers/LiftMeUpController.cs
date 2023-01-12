using LiftMeUp.Data;
using Microsoft.AspNetCore.Mvc;
using LiftMeUp.Areas.Identity.Data;

namespace LiftMeUp.Controllers
{
    public class LiftMeUpController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger<LiftMeUpController> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly LiftMeUpUser _user;

        public LiftMeUpController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<LiftMeUpController> logger)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;

            _user = _context.Users.FirstOrDefault(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }

    }
}