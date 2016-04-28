using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard
{
    public class DashboardOptions
    {
        public DashboardOptions()
        {
            AuthenticationToken = Guid.NewGuid();
        }

        public string HistorianServiceUri { get; set; }

        internal Guid AuthenticationToken { get; private set; }

        public string DashboardUri { get; set; }
    }
}
