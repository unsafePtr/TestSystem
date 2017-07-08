using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Service.Exceptions
{
    public class ActionClaimTypeException : Exception
    {
        public ActionClaimTypeException() : base() { }

        public ActionClaimTypeException(string message) : base(message) { }
    }
}
