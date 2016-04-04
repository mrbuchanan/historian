using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace Historian.Service.Service
{
    internal static class MessageDrop
    {
        public static void Drop(Message message)
        {
            BackgroundJob.Enqueue(() => DoDrop(message));
        }

        [Queue("message_drop")]
        public static void DoDrop(Message message)
        {
            
        }
    }
}
