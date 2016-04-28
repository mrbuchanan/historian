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
                // create lists
                Messages = new List<Message>();
                Channels = new List<Channel>();
                ChannelGroups = new List<ChannelGroup>();

                // add default channel group
                ChannelGroups.Add(new ChannelGroup()
                {
                    Id = Guid.NewGuid(),
                    Name = CatchAllGroup
                });
            }

            /// <summary>
            /// All messages
            /// </summary>
            public List<Message> Messages { get; set; }

            /// <summary>
            /// All Channels
            /// </summary>
            public List<Channel> Channels { get; set; }

            /// <summary>
            /// All Channel Groups
            /// </summary>
            public List<ChannelGroup> ChannelGroups { get; set; }

            /// <summary>
            /// Convert the model to XML
            /// </summary>
            /// <param name="backup">The state to convert</param>
            /// <returns>The state as XML</returns>
            public static string ToXml(State backup)
            {
                // create serializer
                var serializer = new XmlSerializer(typeof(State));

                // create string builder
                var sb = new StringBuilder();

                // serialise model
                using (var writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, backup);
                }

                // return as XML
                return sb.ToString();
            }

            /// <summary>
            /// Convert XML to model
            /// </summary>
            /// <param name="xml">The xml to convert</param>
            /// <returns>The model</returns>
            public static State FromXml(string xml)
            {
                // create serializer
                var serializer = new XmlSerializer(typeof(State));
                object output = null;

                // convert xml
                using (var reader = new StringReader(xml))
                {
                    output = serializer.Deserialize(reader);
                }

                // return state
                return (State) output;
            }
        }

        public MemoryWBackupLogger(ILoggerConfiguration configuration)
        {
            // make sure we have configuration
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // keep internal configuration
            _configuration = configuration;
        }

        public void Log(Message message)
        {
            // check inputs
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (!message.Check()) throw new InvalidMessageException();

            // see if we have an existing channel
            var existingChannel = _state.Channels.FirstOrDefault(c => c.Name.ToLower() == message.Channel.ToLower());

            // if not, create it
            if(existingChannel == null)
            {
                _state.Channels.Add(new Channel()
                {
                    Name = message.Channel,
                    Id = Guid.NewGuid(),
                    Group = CatchAllGroup
                });
            }

            // add the message
            _state.Messages.Add(message);
        }

        public void Log(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            // check inputs
            if (string.IsNullOrWhiteSpace(title)) title = kind.ToString();

            // create message
            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title
            };

            // check and add tags
            if (tags != null) message.Tags = tags.ToList();

            // log the message
            Log(message);
        }

        public IEnumerable<Message> GetMessages(string channel, MessageKind? kind = null, params string[] tags)
        {
            // get all messages for the given channel
            var messages = _state.Messages.Where(m => m.Channel.ToLower() == channel.ToLower());

            // if kind is specified filter
            if(kind != null)
            {
                messages = messages.Where(m => m.Kind == kind.Value);
            }

            // if tags are specified, filter
            if(tags != null && tags.Length > 0)
            {
                messages = messages.Where(m => m.Tags != null && m.Tags.Any(t => tags.Contains(t)));
            }

            // return found messages
            return messages;
        }

        public IEnumerable<Channel> GetChannels(string channelGroup = null)
        {
            // check if we have a channel group and filter if necessary
            if (string.IsNullOrWhiteSpace(channelGroup)) return _state.Channels;

            // return filtered group
            return _state.Channels.Where(c => c.Group.ToLower() == channelGroup.ToLower());
        }

        public IEnumerable<ChannelGroup> GetChannelGroups()
        {
            // return all channel groups
            return _state.ChannelGroups;
        }

        public void Start()
        {
            // check if the file for persisting stage exists
            if (File.Exists(_configuration.Connection))
            {
                // read the file
                var xml = File.ReadAllText(_configuration.Connection);

                // convert to model
                _state = State.FromXml(xml);
            }
            else
            {
                // if no file, create new state
                _state = new State();
                _state.Setup();
            }
        }

        public void Stop()
        {
            // convert model to xml
            var xml = State.ToXml(_state);

            // write xml to file
            File.WriteAllText(_configuration.Connection, xml);
        }
    }
}
