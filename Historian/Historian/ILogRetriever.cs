using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface ILogRetriever
    {
        /// <summary>
        /// Get all messages for a channel optionally filtering by kind and tags
        /// </summary>
        /// <param name="channel">The channel to get messages for</param>
        /// <param name="kind">The kind of messages to get</param>
        /// <param name="tags">The tags to filter messages by</param>
        /// <returns>Any messages found</returns>
        IEnumerable<Message> GetMessages(string channel, MessageKind? kind = null, params string[] tags);

        /// <summary>
        /// Get all channels, optionally specifying a channel group
        /// </summary>
        /// <param name="channelGroup">The channel group to get channels for</param>
        /// <returns>Any channels found</returns>
        IEnumerable<Channel> GetChannels(string channelGroup = null);

        /// <summary>
        /// Get all known channel groups
        /// </summary>
        /// <returns>Any channel groups found</returns>
        IEnumerable<ChannelGroup> GetChannelGroups();
    }
}
