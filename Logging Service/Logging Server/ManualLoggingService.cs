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
        }

        protected override void OnStop()
        {
        }
    }
}
