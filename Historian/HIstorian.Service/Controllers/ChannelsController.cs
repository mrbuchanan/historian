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
        private readonly ILogger _logger;
        private readonly ILogRetriever _logRetriever;

        public ChannelsController(ILogger logger, ILogRetriever logRetriever)
        {
            _logger = logger;
            _logRetriever = logRetriever;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Channel> All()
        {
            return _logRetriever.GetChannels();
        }

        [HttpGet]
        [Route("{channel}/messages/all")]
        public IEnumerable<Message> AllForChannel(string channel)
        {
            return _logRetriever.GetMessages(channel);
        }

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
