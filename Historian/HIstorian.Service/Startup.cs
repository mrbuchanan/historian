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

namespace Historian.Service
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host.
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var configuration = new LoggerConfiguration();

            app.UseNinjectMiddleware(() => CreateKernel(configuration)).UseNinjectWebApi(config);
        }

        private static IKernel CreateKernel(ILoggerConfiguration configuration)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<ILogger>().To<MemoryLogger>().InSingletonScope().WithConstructorArgument("configuration", configuration);
            kernel.Bind<ILogRetriever>().To<MemoryLogger>().InSingletonScope().WithConstructorArgument("configuration", configuration);

            return kernel;
        }
    }
}
