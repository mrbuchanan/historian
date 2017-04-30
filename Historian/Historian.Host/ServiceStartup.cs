using Historian.Api;
using Owin;

namespace Historian.Host
{
    public class ServiceStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // create logger configuration
            var loggerConfiguration = new LoggerConfiguration()
            {
                Connection = @"C:\loggerbackup.xml"
            };

            // get logger type
            var loggerType = typeof(Historian.Loggers.MemoryWBackupLogger);
            
            app.UseHistorianApi(new HistorianApiOptions()
            {
                LoggerType = loggerType,
                LoggerConfiguration = loggerConfiguration,
                UseHangFire = false
            });
        }
    }
}
