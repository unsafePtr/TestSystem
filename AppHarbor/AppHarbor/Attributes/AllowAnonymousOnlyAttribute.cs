using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppHarbor.Attributes
{
    public class AllowAnonymousOnlyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return !httpContext.User.Identity.IsAuthenticated;
        }
    }
}