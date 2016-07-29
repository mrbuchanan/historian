using Historian.Service;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var options = new StartOptions()
            {
                Port = 6666
            };

            using (WebApp.Start<ServiceStartup>("http://+:8081"))
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri("http://localhost:8081/api/")
                };

                Console.WriteLine("bob!");
                Console.ReadLine();
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HistorianService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
