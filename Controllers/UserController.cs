using LiftMeUp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using LiftMeUp.Controllers;
using LiftMeUp.Data;
using LiftMeUp.Areas.Identity.Data;

namespace LiftMeUp.Controllers
{
        [Authorize(Roles = "Admin")]
        public class UsersController : LiftMeUpController
        {

            public UsersController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<LiftMeUpController> logger)
                : base(context, httpContextAccessor, logger)
            {
            }

            public IActionResult Index(string userName, string userId, string firstName, string lastName, string email)
            {
                List<UserViewModel> vmUsers = new List<UserViewModel>();
            List<LiftMeUpUser> users = _context.Users
                                                .Where(u => u.UserName != "null"
                                                        && (u.UserName.Contains(userName) || string.IsNullOrEmpty(userName))
                                                        && (u.Id.Contains(userId) || string.IsNullOrEmpty(userId))
                                                        && (u.FirstName.Contains(firstName) || string.IsNullOrEmpty(firstName))
                                                        && (u.LastName.Contains(lastName) || string.IsNullOrEmpty(lastName))
                                                        && (u.Email.Contains(email) || string.IsNullOrEmpty(email)))                                                                                                       
                                                    .ToList();
                foreach (LiftMeUpUser user in users)
                {
                    vmUsers.Add(new UserViewModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        UserId = user.Id,
                        Roles = (from userRole in _context.UserRoles
                                 where userRole.UserId == user.Id
                                 orderby userRole.RoleId
                                 select userRole.RoleId).ToList()
                    });
                }
                ViewData["userName"] = userName;
                ViewData["userId"] = userId;
                ViewData["firstName"] = firstName;
                ViewData["lastName"] = lastName;
                ViewData["email"] = email;
            return View(vmUsers);
            }

            public IActionResult Undelete(string userName)
            {
                LiftMeUpUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            public IActionResult Delete(string userName)
            {
                LiftMeUpUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                _context.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }


            public IActionResult Roles(string userName)
            {

                LiftMeUpUser user = _context.Users.FirstOrDefault(u => u.UserName == userName);
                RoleViewModel rvm = new RoleViewModel()
                {
                    UserName = userName,
                    Roles = (from userRole in _context.UserRoles
                    where userRole.UserId == user.Id
                             orderby userRole.RoleId
                             select userRole.RoleId).ToList()
                };

                ViewData["RoleIds"] = new MultiSelectList(_context.Roles.OrderBy(c => c.Name), "Id", "Name", rvm.Roles);
                return View(rvm);
            }

            [HttpPost]
            public IActionResult Roles([Bind("UserName,Roles")] RoleViewModel _model)
            {
                LiftMeUpUser user = _context.Users.FirstOrDefault(u => u.UserName == _model.UserName);
                List<IdentityUserRole<string>> userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
                foreach (IdentityUserRole<string> ur in userRoles)
                {
                    _context.Remove(ur);
                }
                if (_model.Roles != null)
                    foreach (string roleId in _model.Roles)
                        _context.UserRoles.Add(new IdentityUserRole<string>() { RoleId = roleId, UserId = user.Id });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

        }
}
