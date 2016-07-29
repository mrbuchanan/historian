using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers.SqlLogger.Entities
{
    internal class Message
    {
        public int Id { get; set; }
        
        public string Contents { get; set; }

        public MessageKind Kind { get; set; }

        public string KindName { get { return Kind.ToString(); } }

        public int ChannelId { get; set; }

        public Channel Channel { get; set; }

        public string Title { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
