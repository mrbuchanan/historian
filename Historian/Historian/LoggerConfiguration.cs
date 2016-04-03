using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public sealed class LoggerConfiguration : ILoggerConfiguration
    {
        public string Connection { get; set; }
    }
}
