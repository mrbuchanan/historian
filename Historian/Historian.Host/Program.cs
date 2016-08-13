using Historian.Api;
using Historian.Service;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageService = WebApp.Start<ServiceStartup>("http://+:8081");
            var dashboard = WebApp.Start<DashboardStartup>("http://+:8082");

            Console.WriteLine("Historian Started");
            Console.WriteLine("Service: http://localhost:8081/");
            Console.WriteLine("Dashboard: http://localhost:8082/");

            Console.ReadLine();

            messageService.Dispose();
            dashboard.Dispose();

            Console.WriteLine("Historian Stopped, press any key to exit.");
        }
    }
}
