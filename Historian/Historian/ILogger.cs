using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public interface ILogger
    {
        void Log(Message message);

        void Log(string message, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags);
    }
}
