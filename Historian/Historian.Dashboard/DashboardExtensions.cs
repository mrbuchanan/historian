using Owin;
using Historian.Dashboard.Dashboard.Content;

namespace Historian.Dashboard
{
    public static class DashboardExtensions
    {
        /// <summary>
        /// Host the Historian Dashboard, using the provided options
        /// </summary>
        /// <param name="app">The App Builder to use</param>
        /// <param name="options">The options to configure the Dashboard</param>
        public static void UseHistorianDashboard(this IAppBuilder app, DashboardOptions options)
        {
            // host content (images etc)
            app.HostContent(options);

            // host webservice pass through
            app.HostWebServicePassthrough("/ws-passthrough");

            // host the dashboard page
            app.HostPage("/dashboard", "Dashboard.Content.html.dashboard.html", new
            {
                historianServiceUri = options.HistorianServiceUri,
                baseUrl = options.DashboardUri,
                bypassLandingPage = options.BypassLandingPage
            });

            // host the new dashboard page
            app.HostPage("/dashboard/v2", "Dashboard.Angular.html.dashboard.html", new
            {
                historianServiceUri = options.HistorianServiceUri,
                baseUrl = options.DashboardUri
            });

            // get the landing page
            var landingPage = options.BypassLandingPage ? 
                              "Dashboard.Content.html.landing.redirect.html" : 
                              "Dashboard.Content.html.landing.html";

            // host the landing page
            app.HostPage("/", landingPage, new
            {
                historianServiceUri = options.HistorianServiceUri,
                baseUrl = options.DashboardUri
            });
        }
    }
}
