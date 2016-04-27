using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    [Serializable]
    public class ChannelGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Channel> Channels { get; }
    }
}
