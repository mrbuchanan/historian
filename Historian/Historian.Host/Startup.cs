using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Historian.Dashboard;

[assembly: OwinStartup(typeof(Historian.Host.DashboardStartup))]

namespace Historian.Host
{
    public class DashboardStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // host dashboard
            app.HostDashboard(new DashboardOptions()
            {
                DashboardUri = "http://localhost:8082",
                HistorianServiceUri = "http://localhost:8081",
                BypassLandingPage = true
            });
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
