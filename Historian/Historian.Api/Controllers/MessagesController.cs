using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Historian.Api.Service;

namespace Historian.Api.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        private readonly ILogger _logger;
        private readonly ILogRetriever _logRetriever;

        public MessagesController(ILogger logger, ILogRetriever logRetriever)
        {
            _logger = logger;
            _logRetriever = logRetriever;
        }

        [HttpPost]
        [Route("drop")]
        public void DropMessage([FromBody] Message message)
        {
            MessageDrop.Drop(message, _logger);
        }
    }
}
