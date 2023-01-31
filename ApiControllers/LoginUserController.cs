using LiftMeUp.ApiLoginUserModel;
using LiftMeUp.Areas.Identity.Data;
using LiftMeUp.Controllers;
using LiftMeUp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LiftMeUp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginUserController : ControllerBase
    {
        private readonly SignInManager<LiftMeUpUser> signInManager;
        private readonly ApplicationDbContext context;

        public LoginUserController(SignInManager<LiftMeUpUser> signInManager, ApplicationDbContext context)
        {
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Boolean>> Login(LoginUserModel model)
        {
            var signInResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);
            if (signInResult.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
