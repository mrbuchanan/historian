using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace Historian.Api.Service
{
    internal static class MessageDrop
    {
        public static bool HangFireEnabled { get; set; }

        public static void Drop(Message message, ILogger logger)
        {
            if (HangFireEnabled) BackgroundJob.Enqueue(() => DoDrop(message, logger));
            else DoDrop(message, logger);
        }

        [Queue("message_drop")]
        public static void DoDrop(Message message, ILogger logger)
        {
            logger.Log(message);
        }
    }
}
