using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Historian.Service.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardChannelsController : ApiController
    {
        private readonly ILogger _logger;
        private readonly ILogRetriever _logRetriever;

        public DashboardChannelsController(ILogger logger, ILogRetriever logRetriever)
        {
            _logger = logger;
            _logRetriever = logRetriever;
        }

        [HttpGet]
        [Route("channels/{channel}/messages/mostRecent/{number}")]
        public IEnumerable<Message> MostRecent(string channel, int number)
        {
            var messages = _logRetriever.GetMessages(channel);
            messages = messages.OrderByDescending(m => m.Timestamp);
            return messages.Take(number);
        }
    }
}
