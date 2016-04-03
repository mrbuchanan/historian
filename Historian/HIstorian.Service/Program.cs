using Historian.Service;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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

            WebApp.Start<Startup>("http://+:8081");

            Console.WriteLine("bob!");
            Console.ReadLine();
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
