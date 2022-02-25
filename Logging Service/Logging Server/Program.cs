/*
Programmers: Amritpal Singh, Dhruvanshi Ghiya
File Name: Program.cs
Project: Logging Server
Solution: Logging Service
Date: 25 Feburary, 2022
Description: 

Reference: https://www.c-sharpcorner.com/article/create-windows-services-in-c-sharp/  (Learned and brushed up my skills on How to build a Window Service)
*/








using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Logging_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ManualLoggingService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
