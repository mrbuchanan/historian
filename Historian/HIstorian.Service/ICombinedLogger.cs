using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Service
{
    internal interface ICombinedLogger : ILogger, ILogRetriever
    {
    }
}
