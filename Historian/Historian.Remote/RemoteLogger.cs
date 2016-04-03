using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Remote
{
    public sealed class RemoteLogger : IRemoteLogger
    {
        private readonly IRemoteLoggerConfiguration _configuration;

        public RemoteLogger(IRemoteLoggerConfiguration config)
        {
            _configuration = config;

            if (_configuration == null) throw new ArgumentNullException(nameof(config));           
        }

        public void Log(Message message)
        {
            SendMessage(message, _configuration);
        }

        public void Log(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            if (string.IsNullOrEmpty(title)) title = kind.ToString();

            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title,
                Tags = tags
            };

            SendMessage(message, _configuration);
        }

        private void SendMessage(Message message, IRemoteLoggerConfiguration configuration)
        {
            BackgroundJob.Enqueue(() => DoSend(message, configuration));
        }

        public void DoSend(Message message, IRemoteLoggerConfiguration configuration)
        {
            var client = new HttpClient()
            {
                BaseAddress = configuration.Endpoint
            };

            var json = JsonConvert.SerializeObject(message);

            var content = new StringContent(json);

            var response = client.PostAsync(configuration.MessageDropPath, content);

            response.Wait();
        }

        public async Task DoSendAsync(Message message, IRemoteLoggerConfiguration configuration)
        {
            var client = new HttpClient()
            {
                BaseAddress = configuration.Endpoint
            };

            var json = JsonConvert.SerializeObject(message);

            var content = new StringContent(json);

            var response = await client.PostAsync(configuration.MessageDropPath, content);
        }

        public async Task LogAsync(Message message)
        {
            await DoSendAsync(message, _configuration);
        }

        public async Task LogAsync(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            if (string.IsNullOrEmpty(title)) title = kind.ToString();

            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title,
                Tags = tags
            };

            await DoSendAsync(message, _configuration);
        }
    }
}
