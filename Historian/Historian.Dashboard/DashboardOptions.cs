using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard
{
    /// <summary>
    /// Options for hosting the Historian Dashboard
    /// </summary>
    public class DashboardOptions
    {
        public DashboardOptions()
        {
            AuthenticationToken = Guid.NewGuid();
        }

        /// <summary>
        /// The URI that the Historian API is located at
        /// </summary>
        public string HistorianServiceUri { get; set; }

        /// <summary>
        /// The authentication token to use for talking to the API
        /// </summary>
        internal Guid AuthenticationToken { get; private set; }

        /// <summary>
        /// The URI to host the dashboard on
        /// </summary>
        public string DashboardUri { get; set; }

        /// <summary>
        /// Whether or not to bypass the landing page and go straight to the channels page
        /// </summary>
        public bool BypassLandingPage { get; set; }
    }
}
