using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Loggers.SqlLogger.Entities
{
    internal class SqlLoggerContext : DbContext
    {
        public SqlLoggerContext() : base()
        {

        }

        public SqlLoggerContext(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {

        }

        public IDbSet<Channel> Channels { get; set; }

        public IDbSet<ChannelGroup> ChannelGroups { get; set; }

        public IDbSet<Message> Messages { get; set; }
    }
}
