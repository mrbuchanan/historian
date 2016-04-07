using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Historian.Loggers
{
    public class MemoryWBackupLogger : IStartableLogger
    {
        private const string CatchAllGroup = "Un-Filed";

        private readonly ILoggerConfiguration _configuration;
        private State _state;

        public class State
        {
            public void Setup()
            {
                Messages = new List<Message>();
                Channels = new List<Channel>();
                ChannelGroups = new List<ChannelGroup>();

                ChannelGroups.Add(new ChannelGroup()
                {
                    Id = Guid.NewGuid(),
                    Name = CatchAllGroup
                });
            }

            public List<Message> Messages { get; set; }
            public List<Channel> Channels { get; set; }
            public List<ChannelGroup> ChannelGroups { get; set; }

            public static string ToXml(State backup)
            {
                var serializer = new XmlSerializer(typeof(State));

                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, backup);
                }

                return sb.ToString();
            }

            public static State FromXml(string xml)
            {
                var serializer = new XmlSerializer(typeof(State));
                var sb = new StringBuilder();
                object output = null;

                using (var reader = new StringReader(xml))
                {
                    output = serializer.Deserialize(reader);
                }

                if (output == null) return null;
                return (State)output;
            }
        }

        public MemoryWBackupLogger(ILoggerConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;
        }

        public void Log(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!message.Check()) throw new InvalidMessageException();

            var existingChannel = _state.Channels.FirstOrDefault(c => c.Name.ToLower() == message.Channel.ToLower());

            if(existingChannel == null)
            {
                _state.Channels.Add(new Channel()
                {
                    Name = message.Channel,
                    Id = Guid.NewGuid(),
                    Group = CatchAllGroup
                });
            }

            _state.Messages.Add(message);
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
                Tags = tags
            };

            Log(message);
        }

        public IEnumerable<Message> GetMessages(string channel, MessageKind? kind = null, params string[] tags)
        {
            var messages = _state.Messages.Where(m => m.Channel.ToLower() == channel.ToLower());

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
            if (string.IsNullOrWhiteSpace(channelGroup)) return _state.Channels;

            return _state.Channels.Where(c => c.Group.ToLower() == channelGroup.ToLower());
        }

        public IEnumerable<ChannelGroup> GetChannelGroups()
        {
            return _state.ChannelGroups;
        }

        public void Start()
        {
            if (File.Exists(_configuration.Connection))
            {
                var serializer = new XmlSerializer(typeof(State));
                var xml = File.ReadAllText(_configuration.Connection);

                _state = State.FromXml(xml);
            }
            else
            {
                _state = new State();
                _state.Setup();
            }
        }

        public void Stop()
        {
            var xml = State.ToXml(_state);

            File.WriteAllText(_configuration.Connection, xml);
        }
    }
}
