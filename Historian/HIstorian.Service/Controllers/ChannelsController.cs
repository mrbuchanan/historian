using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Historian.Service.Controllers
{
    [RoutePrefix("api/channels")]
    public class ChannelsController : ApiController
    {
        [HttpGet]
        [Route("test")]
        public string[] GetTest()
        {
            return new[]
            {
                "Value 1",
                "Value 2"
            };
        }
    }
}
