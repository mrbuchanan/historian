using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface ILoggerConfiguration
    {
        string Connection { get; set; }
    }
}
