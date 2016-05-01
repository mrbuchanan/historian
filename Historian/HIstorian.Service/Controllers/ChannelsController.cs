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
        [Route("groups")]
        public IEnumerable<ChannelGroup> AllGroups()
        {
            return _logRetriever.GetChannelGroups();
        }

        [HttpGet]
        [Route("{channel}/messages/all")]
        public IEnumerable<Message> AllForChannel(string channel)
        {
            return _logRetriever.GetMessages(channel);
        }

        [HttpGet]
        [Route("{channel}/messages/startingAt/{from}")]
        public IEnumerable<Message> StartingAt(string channel, DateTime from)
        {
            var messages = _logRetriever.GetMessages(channel);
            messages = messages.OrderByDescending(m => m.Timestamp);
            return messages.Where(m => m.Timestamp >= from);
        }

        [HttpGet]
        [Route("{channel}/messages/by-kind/{kind}")]
        public IEnumerable<Message> AllForKind(string channel, string kind)
        {
            var messageKind = MessageKind.Information;
            var success = Enum.TryParse(kind, out messageKind);
            if(!success) messageKind = MessageKind.Information;

            return _logRetriever.GetMessages(channel, messageKind);
        }

        [HttpGet]
        [Route("{channel}/messages/by-tag/{tag}")]
        public IEnumerable<Message> AllForTag(string channel, string tag)
        {
            return _logRetriever.GetMessages(channel, null, tag);
        } 
    }
}
