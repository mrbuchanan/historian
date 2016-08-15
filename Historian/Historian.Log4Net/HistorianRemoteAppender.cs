using log4net.Appender;
using System;
using System.Threading.Tasks;
using log4net.Core;
using Historian.Remote;

namespace Historian.Log4Net
{
    public class HistorianRemoteAppender : AppenderSkeleton
    {
        private string _endpoint;
        private string _channel;
        private string _application;
        private IRemoteLogger _remoteLogger;

        protected override void Append(LoggingEvent loggingEvent)
        {
            // send message in a seperate thread
            Task.Run(() => SendMessage(loggingEvent));
        }

        /// <summary>
        /// Send a message to the Historian API
        /// </summary>
        /// <param name="loggingEvent">The Log4Net event to send</param>
        private void SendMessage(LoggingEvent loggingEvent)
        {
            // setup the logger if it needs it
            SetupLogger();

            // get message kind
            var level = GetMessageKind(loggingEvent.Level);

            // get message
            var message = RenderLoggingEvent(loggingEvent);

            // create message to send
            var toSend = new Message()
            {
                Channel = _channel,
                Contents = message,
                Kind = level,
                Timestamp = DateTime.Now,
                Title = "Event From " + _application
            };

            // send
            _remoteLogger.Log(toSend);
        }

        /// <summary>
        /// Setup the logger and update configuration if necessary
        /// </summary>
        private void SetupLogger()
        {
            // get the current endpoint uri
            var endpointUri = new Uri(_endpoint);
            var newLogger = false;

            // check if logger is null
            if(_remoteLogger == null) newLogger = true;
            else
            {
                // get loaded logger config
                var loadedConfig = _remoteLogger.GetConfiguration();

                // check if the connection is different
                if (loadedConfig.Connection != _endpoint) newLogger = true;
            }

            // create a new logger if we need to
            if(newLogger) _remoteLogger = new RemoteLogger(new RemoteLoggerConfiguration(endpointUri));
        }

        /// <summary>
        /// Get a Historian MessageKind from a Log4Net Level
        /// </summary>
        /// <param name="alertLevel">The level of the given Alert</param>
        /// <returns>The appropriate Historian MessageKind value</returns>
        private MessageKind GetMessageKind(Level alertLevel)
        {
            // get information messages
            if (alertLevel.Value == Level.Info.Value) return MessageKind.Information;

            // get debug messages
            if (alertLevel.Value == Level.Debug.Value) return MessageKind.Debug;
            if (alertLevel.Value == Level.Trace.Value) return MessageKind.Debug;

            // get error messages
            if (alertLevel.Value == Level.Error.Value) return MessageKind.Error;

            // get warning messages
            if (alertLevel.Value == Level.Warn.Value) return MessageKind.Warning;

            // get critical messages
            if (alertLevel.Value == Level.Emergency.Value) return MessageKind.WTF;
            if (alertLevel.Value == Level.Critical.Value) return MessageKind.WTF;
            if (alertLevel.Value == Level.Fatal.Value) return MessageKind.WTF;

            // otherwise return as information
            return MessageKind.Information;
        }
        /// <summary>
        /// The endpoint for the Messages to be sent to
        /// </summary>
        public string Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }

        /// <summary>
        /// The channel to send messages to
        /// </summary>
        public string Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        /// <summary>
        /// The name of the application messages are being sent from
        /// </summary>
        public string Application
        {
            get { return _application; }
            set { _application = value; }
        }
    }
}
