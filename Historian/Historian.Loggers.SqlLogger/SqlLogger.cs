using Historian.Loggers.SqlLogger.Entities;
using Historian.Loggers.SqlLogger.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers.SqlLogger
{
    public class SqlLogger : IStartableLogger
    {
        private readonly ILoggerConfiguration _configuration;
        private readonly Mapper _mapper;

        public SqlLogger(ILoggerConfiguration configuration)
        {
            // if no config, throw exception
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // set config
            _configuration = configuration;

            // create mapper
            _mapper = new Mapper();
        }

        public IEnumerable<ChannelGroup> GetChannelGroups()
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // get all groups
            var groups = context.ChannelGroups.ToList();

            // create list
            var toReturn = new List<ChannelGroup>();

            // map to output
            foreach (var g in groups) toReturn.Add(_mapper.ToChannelGroup(g));

            // return to client
            return toReturn;
        }

        public IEnumerable<Channel> GetChannels(string channelGroup = null)
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // get channels
            IEnumerable<Entities.Channel> channels;

            if(string.IsNullOrWhiteSpace(channelGroup))
                channels = context.Channels.ToList();
            else
            {
                var group = context.ChannelGroups.FirstOrDefault(c => c.Name == channelGroup);
                channels = context.Channels.Where(c => c.ChannelGroupId == group.Id).ToList();
            }

            // create temp list
            var toReturn = new List<Channel>();

            // map channels
            foreach (var c in channels)
            {
                if (c.ChannelGroup == null) c.ChannelGroup = context.ChannelGroups.FirstOrDefault(cg => cg.Id == c.ChannelGroupId);
                toReturn.Add(_mapper.ToChannel(c));
            }

            // return to client
            return toReturn;
        }

        public IEnumerable<Message> GetMessages(string channelName, MessageKind? kind = default(MessageKind?), params string[] tags)
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // create return list
            var toReturn = new List<Message>();

            // find channel
            var channel = context.Channels.FirstOrDefault(c => c.Name == channelName);

            // check if channel is present
            if (channel == null) return toReturn;

            // get messages by channel
            var messages = from m in context.Messages
                           where m.ChannelId == channel.Id && (kind != default(MessageKind?) ? m.Kind == kind : true)
                           select m;

            // map messages
            foreach (var m in messages) toReturn.Add(_mapper.ToMessage(m));

            // return messages
            return toReturn;
        }

        public void Log(Message message)
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // add channel if it doesn't exist
            AddOrUpdateChannel(message.Channel, context);

            // find channel
            var channel = context.Channels.FirstOrDefault(c => c.Name == message.Channel);

            // log error for channel not found
            if (channel == null) Log($"Channel not found - {message.Channel}", "historian.errors", MessageKind.Error, "Channel Not Found");

            // set timestamp, if not set
            if (message.Timestamp == default(DateTime)) message.Timestamp = DateTime.Now;

            // create message
            var m = new Entities.Message
            {
                ChannelId = channel.Id,
                Contents = message.Contents,
                Kind = message.Kind,
                Timestamp = message.Timestamp,
                Title = message.Title
            };

            // add and save message
            context.Messages.Add(m);
            context.SaveChanges();
        }

        public void Log(string message, string channelName, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // add channel if it doesn't exist
            AddOrUpdateChannel(channelName, context);

            // find channel
            var channel = context.Channels.FirstOrDefault(c => c.Name == channelName);

            // log error for channel not found
            if (channel == null) Log($"Channel not found - {channelName}", "historian.errors", MessageKind.Error, "Channel Not Found");

            // create message
            var m = new Entities.Message
            {
                ChannelId = channel.Id,
                Contents = message,
                Kind = kind,
                Timestamp = DateTime.Now,
                Title = title
            };

            // add and save message
            context.Messages.Add(m);
            context.SaveChanges();
        }

        private Entities.ChannelGroup GetDefaultGroup(SqlLoggerContext context)
        {
            var group = context.ChannelGroups.FirstOrDefault(c => c.Name == "General");

            if(group == null)
            {
                group = new Entities.ChannelGroup { Name = "General" };
                context.ChannelGroups.Add(group);
                context.SaveChanges();
            }

            return group;
        }

        private void AddOrUpdateChannel(string name, SqlLoggerContext context)
        {
            var defaultGroup = GetDefaultGroup(context);
            var existing = context.Channels.FirstOrDefault(c => c.Name == name);

            if(existing == null)
            {
                existing = new Entities.Channel
                {
                    ChannelGroup = defaultGroup,
                    Name = name
                };

                context.Channels.Add(existing);
                context.SaveChanges();
            }
        }

        public void Start()
        {
            // create context
            var context = new SqlLoggerContext(_configuration.Connection);

            // create database migrator
            var migrator = new MigrateDatabaseToLatestVersion<SqlLoggerContext, Configuration>();

            // update database to latest version
            migrator.InitializeDatabase(context);
        }

        public void Stop()
        {

        }
    }
}
