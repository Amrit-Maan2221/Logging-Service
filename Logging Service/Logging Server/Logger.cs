/*
Programmers: Amritpal Singh, Dhruvanshi Ghiya
File Name: Logger.cs
Project: Logging Server
Solution: Logging Service
Date: 25 Feburary, 2022
Description: This is the server side of the logger project. This is C# based. 
             This file is resplonsible for server logging. It containg all the functionalities required by the logger.

Reference: Some of the Sockets code from client server connection is taken from previous semester 
Examples of Windows and Mobile Programming Course of Module 6 (Inter-Process Communication)
*/



using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Configuration;

namespace Logging_Server
{
    /*
     class: Logger
    Description: Responsible for logging and handles logging of Server
     */
    class Logger
    {
        /*
        * FUNCTION     :   StartLogging
        * DESCRIPTION  :   This function starts the logging process
        * PARAMETERS   :   void
        * RETURNS      :   void
        */
        public static void StartLogging()
        {
            TcpListener loggingServer = null;
            try
            {
                Int32 serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["server_port"]);
                IPAddress serverIPAddress = IPAddress.Parse(ConfigurationManager.AppSettings["server_ip_address"]);
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




        /*
        * FUNCTION     :   Worker
        * DESCRIPTION  :   Thread function which executes server-client communication
        * PARAMETERS   :   object o    :   Contains socket connection
        * RETURNS      :   void
        */
        internal static void Worker(Object o)
        {
            TcpClient client = (TcpClient)o;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            //we can get the ip address too.........
            string clientIPAddress = client.Client.RemoteEndPoint.ToString();

            int readData = 0;

            while ((readData = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                string clientMessage = System.Text.Encoding.ASCII.GetString(bytes, 0, readData);

                //response message to let the client know we got your message
                string response = "SUCCESS!!!";

                // parsing client message to get formatted log message
                string logMessage = ParseClientMessage(clientMessage, clientIPAddress);

                if (!(logMessage == "Error: Invalid Log Format"))
                {
                    GenerateLogFile(logMessage);
                }

                // Encoding message
                byte[] responseMessage = System.Text.Encoding.ASCII.GetBytes(response);
                stream.Write(responseMessage, 0, responseMessage.Length);
            }

            // Shutdown and end connection
            client.Close();
        }




        /*
        * FUNCTION     :   ParseClientMessage
        * DESCRIPTION  :   function to parse Client messages
        * PARAMETERS   :   string message-> Msg recieved from client and its level
        *                   IP address of Client
        * RETURNS      :   message
        */
        static string ParseClientMessage(string message, string clientIPAddress)
        {
            // Declarations
            int logLevel = 0;
            string logLevelStr = "";

            //Split the message
            var strMessageSeparation = message.Split('#');

            // Check for possible errors
            if (strMessageSeparation.Length != 2 || int.TryParse(strMessageSeparation[0], out logLevel) == false)
            {
                string errMsg = "Error: Invalid Log Format";
                return errMsg;
            }

            string msgInfo = strMessageSeparation[1];

            //Getting the log level in String to parse
            ParseLogLevel(logLevel, ref logLevelStr);

            var parseClientIPAdressAndPort = clientIPAddress.Split(':');

            //Time-stamp parsing
            string timeStamp = DateTime.Now.ToString("MMM dd HH:mm:ss");

            //our log format is date time clientIpAddress Loglevel: Message
            return $"{timeStamp} {parseClientIPAdressAndPort[0]} [{logLevelStr}]: {msgInfo}";
        }




        /*
        * FUNCTION     :   ParseLogLevel
        * DESCRIPTION  :   function to parse Client messages
        * PARAMETERS   :   logLevel, logLevelStr
        * RETURNS      :   message
        */
        static void ParseLogLevel(int logLevel, ref string logLevelStr)
        {
            //"Log Levels: 0. OFF"
            if (logLevel == 0)
            {
                logLevelStr = "OFF";
            }
            //"Log Levels: 1. FATAL
            else if (logLevel == 1)
            {
                logLevelStr = "FATAL";
            }
            //"Log Levels: 2. ERROR
            else if (logLevel == 2)
            {
                logLevelStr = "ERROR";
            }
            //"Log Levels: 3. WARN
            else if (logLevel == 3)
            {
                logLevelStr = "WARNING";
            }
            //"Log Levels: 4. INFO
            else if (logLevel == 4)
            {
                logLevelStr = "INFO";
            }
            //"Log Levels: 5.DEBUG
            else if (logLevel == 5)
            {
                logLevelStr = "DEBUG";
            }
            //"Log Levels: 6. TRACE"
            else if (logLevel == 6)
            {
                logLevelStr = "TRACE";
            }
            //"Log Levels: NULL
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
         *  References: Dimitrios Kalemis
         *  https://dkalemis.wordpress.com/2013/06/23/how-to-lock-a-text-file-for-reading-and-writing-using-csharp/
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
