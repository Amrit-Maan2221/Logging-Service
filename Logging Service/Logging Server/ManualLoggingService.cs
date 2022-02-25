using System;
using System.ServiceProcess;

namespace Logging_Server
{
    public partial class ManualLoggingService : ServiceBase
    {
        public ManualLoggingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.GenerateLogFile("Logging Started at" + DateTime.Now);
            Logger.StartLogging();
        }

        protected override void OnStop()
        {
            Logger.GenerateLogFile("Logging Stopped because Server is stopped at " + DateTime.Now);
        }
    }
}
