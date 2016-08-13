using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface ILogger
    {
        /// <summary>
        /// Get the Loggers Configuration
        /// </summary>
        /// <returns></returns>
        ILoggerConfiguration GetConfiguration();

        /// <summary>
        /// Drop a message into the logger
        /// </summary>
        /// <param name="message">The message to add</param>
        void Log(Message message);

        /// <summary>
        /// Drop a message into the logger
        /// </summary>
        /// <param name="message">The message contents</param>
        /// <param name="channel">The channel for the message to be assigned to</param>
        /// <param name="kind">The kind of the message, defaults to Information</param>
        /// <param name="title">The title of the message</param>
        /// <param name="tags">Any tags this message should be tagged with</param>
        void Log(string message, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags);
    }
}
