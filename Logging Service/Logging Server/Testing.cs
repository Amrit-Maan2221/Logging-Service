

using System;

namespace Logging_Server
{
    class Testing
    {
        static void Main(string[] args)
        {
            Logger.GenerateLogFile("Logging Service Started at" + DateTime.Now);
            Logger.StartLogging();
        }
    }
}
