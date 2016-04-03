using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Remote
{
    public interface IRemoteLogger : ILogger
    {
        Task LogAsync(Message message);

        Task LogAsync(string message, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags);
    }
}
