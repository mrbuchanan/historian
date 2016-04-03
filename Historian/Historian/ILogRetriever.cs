using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface ILogRetriever
    {
        IEnumerable<Message> GetMessages(string channel, MessageKind? kind = null, params string[] tags);

        IEnumerable<Channel> GetChannels(string channelGroup = null);

        IEnumerable<ChannelGroup> GetChannelGroups();
    }
}
