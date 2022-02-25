/*
Programmers: Amritpal Singh, Dhruvanshi Ghiya
File Name: Logger.cs
Project: Logging Server
Solution: Logging Service
Date: 25 Feburary, 2022
Description: 

Reference: Some of the Sockets code from client server connection is taken from previous semester Examples of Windows and Mobile Programming Course of Module 6 
(Inter-Process Communication)
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
                //later will get these value from App.config...todo
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
                    break;
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
            TcpClient client = (TcpClient)o;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            string clientMessage = null;
            
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            //we can get the ip address too.........
            //string str = client.Client.RemoteEndPoint.ToString();

            int readData = 0;

            while ((readData = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                clientMessage = System.Text.Encoding.ASCII.GetString(bytes, 0, readData);

                //response message to let the client know we got your message
                string response = "SUCCESS!!!";

                // parsing client message to get formatted log message
                string logMessage = ParseClientMessage(clientMessage);

                if (!(logMessage == "Error: Invalid Log Format"))
                {
                    GenerateLogFile(logMessage);
                }

                byte[] responseMessage = System.Text.Encoding.ASCII.GetBytes(response);
                stream.Write(responseMessage, 0, responseMessage.Length);
            }

            // Shutdown and end connection
            client.Close();
        }





        static string ParseClientMessage(string message)
        {
            int logLevel = 0;
            string logLevelStr = "";

            var strMessageSeparation = message.Split('#');

            if (strMessageSeparation.Length != 2 || int.TryParse(strMessageSeparation[0], out logLevel) == false)
            {
                string errMsg = "Error: Invalid Log Format";
                return errMsg;
            }

            string msgInfo = strMessageSeparation[1];

            //Getting the log level in String
            ParseLogLevel(logLevel, ref logLevelStr);

            string timeStamp = DateTime.Now.ToString("MMM dd HH:mm:ss");
            return $"{timeStamp} [{logLevelStr}]: {msgInfo}";
        }



        static void ParseLogLevel(int logLevel, ref string logLevelStr)
        {
            //"Log Levels:", "0. OFF", "1. FATAL", "2. ERROR", "3. WARN", "4. INFO", "5.DEBUG", "6. TRACE")
            if (logLevel == 0)
            {
                logLevelStr = "OFF";
            }
            else if(logLevel == 1)
            {
                logLevelStr = "FATAL";
            }
            else if (logLevel == 2)
            {
                logLevelStr = "ERROR";
            }
            else if (logLevel == 3)
            {
                logLevelStr = "WARNING";
            }
            else if (logLevel == 4)
            {
                logLevelStr = "INFO";
            }
            else if (logLevel == 5)
            {
                logLevelStr = "DEBUG";
            }
            else if (logLevel == 6)
            {
                logLevelStr = "TRACE";
            }
            else
            {
                logLevelStr = "";
            }
        }



        /*
         * Function Name: GenerateLogFile
         * Parameters: string stringToWrite --> The String that we want to write into the file
         * Description: This function appends the log messages to the file
         * Returns: void
         *  References:
            * TITLE : How to lock a text file for reading and writing using C#
            * AUTHOR : Dimitrios Kalemis
            * DATE : June 23, 2013 
            * AVAILABIILTY : https://dkalemis.wordpress.com/2013/06/23/how-to-lock-a-text-file-for-reading-and-writing-using-csharp/
         */
        public static void GenerateLogFile(string stringToWrite)
        {

            FileStream myFileStream = null;

            try
            {
                // continue loop until file is not in use anymore
                while (true)
                {
                    try
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        // FileShare.Read will lock the file for writing while in use by this process
                        myFileStream = new FileStream(path + "\\log.txt", FileMode.Append, FileAccess.Write, FileShare.None);
                        break;
                    }
                    catch (FileNotFoundException ex)
                    {
                        throw ex;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(20);
                    }
                }

                StreamWriter myStreamWriter = new StreamWriter(myFileStream);

                myStreamWriter.WriteLine(stringToWrite);

                myStreamWriter.Close();
                myFileStream.Close();
                myFileStream.Dispose();

            }
            catch (ThreadAbortException)
            {
                // if thread is aborted
                if (myFileStream != null)
                {
                    myFileStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (myFileStream != null)
                {
                    myFileStream.Dispose();
                }
            }
        }
    }
}
