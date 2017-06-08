using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TestSystem.Service;
using TestSystem.Service.Dtos;

namespace IdentityExample.Controllers
{
    public class ServiceUsageController : ApiController
    {
        public ITestSystemService service;

        public ServiceUsageController(ITestSystemService service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("api/TestService/CreateTest")]
        public async System.Threading.Tasks.Task<IHttpActionResult> GetAsync(TestDto test)
        {
            var testId = await service.CreateTestAsync(test);
            return Ok(testId);
        }
    }
}