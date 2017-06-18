using AppHarbor.Attributes;
using AppHarbor.DbAccess.Entities;
using AppHarbor.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TestSystem.Service;
using TestSystem.Service.Claims;
using System.Threading.Tasks;

namespace AppHarbor.Controllers
{
    public class AccountController : BaseController
    {
        private UserManager<User> UserManager { get; }
        private ITestSystemService TestSystemService { get; }

        public AccountController(UserManager<User> manager, ITestSystemService testSystemService)
        {
            this.UserManager = manager;
            this.TestSystemService = testSystemService;
        }

        [HttpGet]
        [AllowAnonymousOnly]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymousOnly]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = new User()
            {
                UserName = model.UserName,
                TestSystemUserId = await TestSystemService.CreateUserAsync()
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                // just to show features of TestSystemService is allowed to register user with diffrent roles
                var role = model.IsLector ? AppHarborRoles.Lector : AppHarborRoles.Student;
                await UserManager.AddToRoleAsync(user.Id, role.ToString());
                await this.SignIn(user, role);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error, error);
                }

                return View();
            }
        }

        [HttpGet]
        [AllowAnonymousOnly]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymousOnly]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = UserManager.Find(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", "User not found!");
                return View();
            }

            IList<string> userRoles = await UserManager.GetRolesAsync(user.Id);

            await this.SignIn(user, userRoles.Select(r => (AppHarborRoles)Enum.Parse(typeof(AppHarborRoles), r)).ToArray());

            if (returnUrl != null)
            {
                if (returnUrl == "/Account/Logout" || returnUrl == "/Account/Login")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            
            return RedirectToAction("Index", "Home");
        }

        private async Task SignIn(User user, params AppHarborRoles[] roles)
        {
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            Claim[] claims;
            // add claims depending on role type
            if (roles.Any(r => r == AppHarborRoles.Student))
            {
                claims = new Claim[]
                {
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.AddAnswer),
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.AddUserTest),
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.GetUserTest),
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.GetTests),
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.StartUserTest),
                    new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.EndUserTest)
                };
            }
            else
            {
                // add all permissions
                claims = typeof(ActionPermissionValues).GetFields()
                    .Select(field => new Claim(ActionClaimType.ActionPermission, field.Name))
                    .ToArray();
            }

            identity.AddClaims(claims);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }
    }
}