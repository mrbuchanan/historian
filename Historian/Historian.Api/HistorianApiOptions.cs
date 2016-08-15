using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Api
{
    /// <summary>
    /// Options for hosting the Historian API
    /// </summary>
    public class HistorianApiOptions
    {
        /// <summary>
        /// The type of Logger to use.
        /// NB: The assembly the logger is in, must be loaded and referenced
        /// </summary>
        public Type LoggerType { get; set; }

        /// <summary>
        /// The configuration for the logger type provided, this is usually a connection string or file path.
        /// </summary>
        public ILoggerConfiguration LoggerConfiguration { get; set; }

        /// <summary>
        /// Whether or not to use HangFire for message drops in the API
        /// </summary>
        public bool UseHangFire { get; set; }

        /// <summary>
        /// The Connection String for HangFire to use (if enabled).
        /// </summary>
        public string HangFireConnectionString { get; set; }
    }
}
