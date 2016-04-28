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
            // send message
            SendMessage(message, _configuration);
        }

        public void Log(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            // check inputs
            if (string.IsNullOrEmpty(title)) title = kind.ToString();

            // create message
            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title
            };
            
            // check and add tags
            if (tags != null) message.Tags = tags.ToList();

            // send message
            SendMessage(message, _configuration);
        }

        private void SendMessage(Message message, IRemoteLoggerConfiguration configuration)
        {
            // queue send on hangfire
            BackgroundJob.Enqueue(() => DoSend(message, configuration));
        }

        public void DoSend(Message message, IRemoteLoggerConfiguration configuration)
        {
            // create http client
            var client = new HttpClient()
            {
                BaseAddress = configuration.Endpoint
            };

            // serialise to json
            var json = JsonConvert.SerializeObject(message);

            // create http content
            var content = new StringContent(json);

            // post message
            var response = client.PostAsync(configuration.MessageDropPath, content);

            // wait for response
            response.Wait();
        }

        public async Task DoSendAsync(Message message, IRemoteLoggerConfiguration configuration)
        {
            // create http client
            var client = new HttpClient()
            {
                BaseAddress = configuration.Endpoint
            };


            // serialise to json
            var json = JsonConvert.SerializeObject(message);

            // create http content
            var content = new StringContent(json);

            // post message
            var response = await client.PostAsync(configuration.MessageDropPath, content);
        }

        public async Task LogAsync(Message message)
        {
            // send message
            await DoSendAsync(message, _configuration);
        }

        public async Task LogAsync(string contents, string channel, MessageKind kind = MessageKind.Information, string title = null, params string[] tags)
        {
            // check inputs
            if (string.IsNullOrEmpty(title)) title = kind.ToString();

            // create message
            var message = new Message()
            {
                Contents = contents,
                Channel = channel,
                Kind = kind,
                Title = title
            };

            // check and add tags
            if (tags != null) tags.ToList();

            // send message
            await DoSendAsync(message, _configuration);
        }
    }
}
