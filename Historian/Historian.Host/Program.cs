using Historian.Api;
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
        private IDisposable _api;
        private IDisposable _dashboard;

        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        private void Run()
        {
            Start();

            var exit = false;

            while(!exit)
            {
                var entered = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entered))
                {
                    Console.WriteLine("Please enter a valid command:");
                    Console.WriteLine("\trestart");
                    Console.WriteLine("\texit");
                }
                else if (entered.ToLower() == "restart")
                {
                    Stop();
                    Start();
                }
                else if (entered.ToLower() == "exit")
                {
                    Stop();
                    exit = true;
                }
            }
        }

        private void Start()
        {
            _api = WebApp.Start<ServiceStartup>("http://+:8081");
            _dashboard = WebApp.Start<DashboardStartup>("http://+:8082");

            Console.WriteLine("Historian Started");
            Console.WriteLine("Service: http://localhost:8081/");
            Console.WriteLine("Dashboard: http://localhost:8082/");
        }

        private void Stop()
        {
            _api.Dispose();
            _dashboard.Dispose();
            Console.WriteLine("Historian Stopped");
        }
    }
}
