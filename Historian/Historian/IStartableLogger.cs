using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface IStartableLogger : ILogger, ILogRetriever
    {
        /// <summary>
        /// Start the logger and run any initialisation necessary, for example loading state
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the logger and run any shitdown actions necessary, for example saving state
        /// </summary>
        void Stop();
    }
}
