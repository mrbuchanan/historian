using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers
{
    public class MemoryLogger : IStartableLogger
    {
        private const string CatchAllGroup = "Un-Filed";

        private readonly ILoggerConfiguration _configuration;
        private List<Message> _messages;
        private List<Channel> _channels;
        private List<ChannelGroup> _channelGroups;

        public MemoryLogger(ILoggerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
        }

        public void Log(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!message.Check()) throw new InvalidMessageException();

            var existingChannel = _channels.FirstOrDefault(c => c.Name.ToLower() == message.Channel.ToLower());

            if(existingChannel == null)
            {
                _channels.Add(new Channel()
                {
                    Name = message.Channel,
                    Id = Guid.NewGuid(),
                    Group = CatchAllGroup
                });
            }

            _messages.Add(message);
        }

        public void Log(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            if (string.IsNullOrWhiteSpace(title)) title = kind.ToString();

            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title,
                Tags = tags.ToList()
            };

            Log(message);
        }

        public IEnumerable<Message> GetMessages(string channel, MessageKind? kind = null, params string[] tags)
        {
            var messages = _messages.Where(m => m.Channel.ToLower() == channel.ToLower());

            if(kind != null)
            {
                messages = messages.Where(m => m.Kind == kind.Value);
            }

            if(tags != null && tags.Length > 0)
            {
                messages = messages.Where(m => m.Tags != null && m.Tags.Any(t => tags.Contains(t)));
            }

            return messages;
        }

        public IEnumerable<Channel> GetChannels(string channelGroup = null)
        {
            if (string.IsNullOrWhiteSpace(channelGroup)) return _channels;

            return _channels.Where(c => c.Group.ToLower() == channelGroup.ToLower());
        }

        public IEnumerable<ChannelGroup> GetChannelGroups()
        {
            return _channelGroups;
        }

        public void Start()
        {
            _messages = new List<Message>();
            _channels = new List<Channel>();
            _channelGroups = new List<ChannelGroup>();

            _channelGroups.Add(new ChannelGroup()
            {
                Id = Guid.NewGuid(),
                Name = CatchAllGroup
            });
        }

        public void Stop()
        {
            _messages = new List<Message>();
            _channels = new List<Channel>();
            _channelGroups = new List<ChannelGroup>();

            _channelGroups.Add(new ChannelGroup()
            {
                Id = Guid.NewGuid(),
                Name = CatchAllGroup
            });
        }
    }
}
