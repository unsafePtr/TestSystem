using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestSystem.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class ClaimCheckAttribute : Attribute
    {
        private string Type { get; set; }
        private string Value { get; set; }

        public ClaimCheckAttribute(string claimType, string claimValue)
        {
            this.Value = claimValue;
            this.Type = claimType;
        }

        public bool IsAllowedAccess
        {
            get
            {
                ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                return identity.Claims
                    .Where(c => c.Type == Type && c.Value == Value)
                    .Any();
            }
        }
    }
}
