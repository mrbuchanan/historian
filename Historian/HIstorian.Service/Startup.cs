using Historian.Loggers;
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

namespace Historian.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host.
            var webApiConfig = new HttpConfiguration();
            WebApiConfig.Register(webApiConfig);

            // create logger configuration
            var loggerConfiguration = new LoggerConfiguration();

            // setup hangfire storage
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("Hangfire.ConnectionString");

            // show dashboard
            app.UseHangfireDashboard();

            // setup hangfire server
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                // setup the queues
                Queues = new []
                {
                    "message_drop"
                },

                // set good 
                WorkerCount = 4
            });

            // setup webapi to use NInject
            app.UseNinjectMiddleware(() => CreateKernel(loggerConfiguration)).UseNinjectWebApi(webApiConfig);
        }

        private static IKernel CreateKernel(ILoggerConfiguration configuration)
        {
            // setup NInject kernel
            var kernel = new StandardKernel();
            
            // load the current assembly
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<ILogger>().To<MemoryLogger>().InSingletonScope().WithConstructorArgument("configuration", configuration);
            kernel.Bind<ILogRetriever>().To<MemoryLogger>().InSingletonScope().WithConstructorArgument("configuration", configuration);

            return kernel;
        }
    }
}
