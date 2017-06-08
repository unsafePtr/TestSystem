using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using System.Web.Http;

[assembly: OwinStartup(typeof(IdentityExample.Startup))]

namespace IdentityExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/api/Account/SignIn")
            });
        }
    }
}
