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
            app.UseHistorianDashboard(new DashboardOptions()
            {
                DashboardUri = "http://localhost:8082",
                HistorianServiceUri = "http://localhost:8081",
                BypassLandingPage = true
            });
        }
    }
}
