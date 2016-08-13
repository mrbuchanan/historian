using System.Threading.Tasks;

namespace Historian.Remote
{
    public interface IRemoteLogger : ILogger
    {
        /// <summary>
        /// Drop a message into the logger
        /// </summary>
        /// <param name="message">The message to drop</param>
        /// <returns>An awaitable task</returns>
        Task LogAsync(Message message);

        /// <summary>
        /// Drop a message into the logger
        /// </summary>
        /// <param name="message">The message contents</param>
        /// <param name="channel">The channel for the message</param>
        /// <param name="kind">The kind of message, defaults to Information</param>
        /// <param name="title">The title for the message</param>
        /// <param name="tags">Any tags for the message</param>
        /// <returns>An awaitable task</returns>
        Task LogAsync(string message, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags);
    }
}
