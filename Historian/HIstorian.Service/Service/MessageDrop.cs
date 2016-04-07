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
        public static void Drop(Message message, ILogger logger)
        {
            if (Properties.Settings.Default.UseHangFireQueue) BackgroundJob.Enqueue(() => DoDrop(message, logger));
            else DoDrop(message, logger);
        }

        [Queue("message_drop")]
        public static void DoDrop(Message message, ILogger logger)
        {
            logger.Log(message);
        }
    }
}
