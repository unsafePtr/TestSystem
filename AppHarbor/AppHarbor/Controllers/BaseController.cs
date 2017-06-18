using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin;

namespace AppHarbor.Controllers
{
    public class BaseController : Controller
    {
        public IOwinContext OwinContext => this.Request.GetOwinContext();
        public IAuthenticationManager AuthenticationManager => OwinContext.Authentication;
    }
}