using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityExample.Models
{
    public class User : IdentityUser
    {
        public string TestSystemUserId { get; set; }
    }
}