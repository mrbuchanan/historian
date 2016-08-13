using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Remote
{
    public class RemoteLoggerConfiguration : IRemoteLoggerConfiguration
    {
        public RemoteLoggerConfiguration(Uri endpoint)
        {
            Endpoint = endpoint;
            MessageDropPath = "messages/drop";
        }

        public string Connection
        {
            get { return Endpoint.ToString(); }
            set { Endpoint = new Uri(value); }
        }

        public Uri Endpoint { get; private set; }

        public string MessageDropPath { get; private set; }
    }
}
