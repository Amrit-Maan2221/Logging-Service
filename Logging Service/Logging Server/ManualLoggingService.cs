/*
Programmers: Amritpal Singh, Dhruvanshi Ghiya
File Name: ManualLoggingService.cs
Project: Logging Server
Solution: Logging Service
Date: 25 Feburary, 2022
Description: This file conatins that code that should be executed when service start and end and also initilize the service

*/

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
            Logger.GenerateLogFile("---------------Logging Started at " + DateTime.Now);
            Logger.StartLogging();
        }

        protected override void OnStop()
        {
            Logger.GenerateLogFile("---------------Logging Stopped because Server is stopped at " + DateTime.Now);
        }
    }
}
