using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Remote
{
    public interface IRemoteLoggerConfiguration 
    {
        Uri Endpoint { get; }

        string MessageDropPath { get; }
    }
}
