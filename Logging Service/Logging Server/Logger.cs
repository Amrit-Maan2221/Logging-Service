/*
Programmers: Amritpal Singh, Dhruvanshi Ghiya
File Name: Logger.cs
Project: Logging Server
Solution: Logging Service
Date: 25 Feburary, 2022
Description: 

Reference: Some of the Sockets code is taken from previous semester Examples of Windows and Mobile Programming Course of Module 6 (Inter-Process Communication)
*/



using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace Logging_Server
{
    class Logger
    {
        public static void StartLogging()
        {
            TcpListener loggingServer = null;
            try
            {
                //later will get these value from App.config
                Int32 serverPort = Convert.ToInt32("9001");
                IPAddress serverIPAddress = IPAddress.Parse("127.0.0.1");
                loggingServer = new TcpListener(serverIPAddress, serverPort);

                //We will start our server
                loggingServer.Start();

                // Enter the listening loop.
                while (true)
                {
                    // Perform a blocking call to accept requests.
                    TcpClient client = loggingServer.AcceptTcpClient();
                    //ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
                    //Thread clientThread = new Thread(ts);
                    //clientThread.Start(client);
                    Worker(client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                // In the end we stop the server.
                loggingServer.Stop();
            }
        }


        internal static void Worker(Object o)
        {
        }
    }
}
