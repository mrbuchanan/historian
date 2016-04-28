using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Historian.Dashboard.Dashboard.Content;

namespace Historian.Dashboard
{
    public static class DashboardExtensions
    {
        public static void HostDashboard(this IAppBuilder app, DashboardOptions options)
        {
            // host content (images etc)
            app.HostContent(options);

            // host webservice pass through
            app.HostWebServicePassthrough("/ws-passthrough");

            // host the dashboard page
            app.HostPage("/dashboard", "Dashboard.Content.html.dashboard.html", new
            {
                historianServiceUri = options.HistorianServiceUri,
                baseUrl = options.DashboardUri
            });

            // host the landing page
            app.HostPage("/", "Dashboard.Content.html.landing.html", new
            {
                historianServiceUri = options.HistorianServiceUri,
                baseUrl = options.DashboardUri
            });
        }
    }
}
