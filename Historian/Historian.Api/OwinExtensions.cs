using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Historian.Api.Service;
using Microsoft.Owin.BuilderProperties;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace Historian.Api
{
    public static class OwinExtensions
    {
        /// <summary>
        /// Host the Historian API
        /// </summary>
        /// <param name="app">The App Builder to use</param>
        /// <param name="options">The options for setting up the API</param>
        public static void UseHistorianApi(this IAppBuilder app, HistorianApiOptions options)
        {
            // check inputs
            if (options == null) throw new ArgumentNullException(nameof(options));

            // get logger type
            var loggerInstance = (IStartableLogger)Activator.CreateInstance(options.LoggerType, options.LoggerConfiguration);

            // start logger
            loggerInstance.Start();

            // read app properties
            var properties = new AppProperties(app.Properties);

            // get cancellation token
            var token = properties.OnAppDisposing;

            // if we have one
            if (token != CancellationToken.None)
            {
                // register callback to shutdown stuff
                token.Register(() =>
                {
                    // stop logger
                    loggerInstance.Stop();
                });
            }

            // Configure Web API for self-host.
            var webApiConfig = new HttpConfiguration();
            WebApiConfig.Register(webApiConfig);

            // setup hangfire
            if (options.UseHangFire)
            {
                // turn on hangfire for message drop
                MessageDrop.HangFireEnabled = true;

                // get hangfire connection string
                var hangFireConnectionInfo = options.HangFireConnectionString;

                // setup hangfire storage
                Hangfire.GlobalConfiguration.Configuration
                    .UseSqlServerStorage(hangFireConnectionInfo);

                // show dashboard
                app.UseHangfireDashboard();

                // setup hangfire server
                app.UseHangfireServer(new BackgroundJobServerOptions()
                {
                    // setup the queues
                    Queues = new[]
                    {
                        "message_drop"
                    },

                    // set good 
                    WorkerCount = 4
                });
            }

            // setup webapi to use NInject
            app.UseNinjectMiddleware(() => CreateKernel(loggerInstance)).UseNinjectWebApi(webApiConfig);
        }

        private static IKernel CreateKernel(IStartableLogger logger)
        {
            // setup NInject kernel
            var kernel = new StandardKernel();

            // load the current assembly
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<ILogger>().ToMethod((context) => logger);
            kernel.Bind<ILogRetriever>().ToMethod(context => logger);

            return kernel;
        }
    }
}
