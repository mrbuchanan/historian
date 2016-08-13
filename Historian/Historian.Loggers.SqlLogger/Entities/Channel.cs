using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers.SqlLogger.Entities
{
    internal class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ChannelGroupId { get; set; }

        public ChannelGroup ChannelGroup { get; set; }
    }
}
