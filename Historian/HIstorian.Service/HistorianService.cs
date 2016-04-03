using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Service
{
    partial class HistorianService : ServiceBase
    {
        private IDisposable _server;

        public HistorianService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            var options = new StartOptions()
            {
                Port = 6666
            };

            _server = WebApp.Start<Startup>(options);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            if (_server != null) _server.Dispose();

            base.OnStop();
        }
    }
}
