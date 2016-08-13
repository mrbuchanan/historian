using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Api.Service
{
    internal class SetupDatabase
    {
        private string _connectionString;

        public void Setup(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void InitialSetup()
        {
            
        }
    }
}
