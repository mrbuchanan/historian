using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers.SqlLogger.Entities
{
    internal class Mapper
    {
        public Historian.Channel ToChannel(Channel input)
        {
            var channel = new Historian.Channel()
            {
                Id = input.Id,
                Name = input.Name,
                Group = input.ChannelGroup.Name
            };

            return channel;
        }

        public Historian.ChannelGroup ToChannelGroup(ChannelGroup input)
        {
            return new Historian.ChannelGroup()
            {
                Id = input.Id,
                Name = input.Name
            };
        }

        public Historian.Message ToMessage(Message input)
        {
            return new Historian.Message()
            {
                Channel = input.Channel.Name,
                Kind = input.Kind,
                Contents = input.Contents,
                Timestamp = input.Timestamp,
                Title = input.Title
            };
        }
    }
}
