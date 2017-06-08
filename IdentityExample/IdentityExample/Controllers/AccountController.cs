using IdentityExample.Context;
using IdentityExample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using TestSystem.Service;
using TestSystem.Service.Claims;

namespace IdentityExample.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAppDbContext context;
        private readonly UserManager<User> manager;
        private readonly ITestSystemService testSystemService;
        private IAuthenticationManager authenticationManager => Request.GetOwinContext().Authentication;

        public AccountController(
            UserManager<User> manager, 
            IAppDbContext context, 
            ITestSystemService service)
        {
            this.manager = manager;
            this.context = context;
            this.testSystemService = service;
        }

        [HttpPost]
        [Route("api/Account/RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser(UserViewModel user)
        {
            User newUser = new User()
            {
                UserName = user.UserName,
                TestSystemUserId = await testSystemService.CreateUserAsync()
            };

            var result = await manager.CreateAsync(newUser, user.Password);

            if(result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Smth goes wrong");
            }
        }

        [HttpPost]
        [Route("api/Account/SignIn")]
        public async Task<IHttpActionResult> SignIn([FromBody]UserViewModel userAuth)
        {
            User user = await manager.FindAsync(userAuth.UserName, userAuth.Password);
            
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            var identity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            identity.AddClaims(new List<Claim>()
            {
                new Claim(ActionClaimType.ActionPermission, ActionPermissionValues.CreateTest)
            });

            authenticationManager.SignIn(identity);

            return Ok("Successful login");
        }
    }
}
