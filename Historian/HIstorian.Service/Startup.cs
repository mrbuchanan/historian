﻿using Historian.Loggers;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Microsoft.Owin.BuilderProperties;
using System.Threading;
using System.Configuration;

namespace Historian.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // create logger configuration
            var loggerConfiguration = new LoggerConfiguration()
            {
                Connection = Properties.Settings.Default.LoggerConnection
            };

            // get logger type
            var loggerTypeName = Properties.Settings.Default.LoggerType;
            var loggerType = Type.GetType(loggerTypeName);
            var loggerInstance = (IStartableLogger)Activator.CreateInstance(loggerType, loggerConfiguration);

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
            if (Properties.Settings.Default.UseHangFireQueue)
            {
                // get hangfire connection string
                var hangFireConnectionString = ConfigurationManager.ConnectionStrings["HangFire.ConnectionString"];

                // setup hangfire storage
                GlobalConfiguration.Configuration
                    .UseSqlServerStorage(hangFireConnectionString.ConnectionString);

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
