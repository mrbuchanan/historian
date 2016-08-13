using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Remote
{
    public interface IRemoteLoggerConfiguration : ILoggerConfiguration
    {
        Uri Endpoint { get; }

        string MessageDropPath { get; }
    }
}
