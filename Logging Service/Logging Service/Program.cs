using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
// The following code is extracted from the MSDN site:
//https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?view=net-5.0
//
namespace TCPIPServer
{
    class Program
    {

        public static void start()
        {
            Int32 _port = 13000;
            byte[] buffer = new Byte[1024];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket socket = listener.Accept();
                    String data = null;

                    while (true)
                    {
                        int bytesRec = socket.Receive(buffer);
                        data += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    Console.WriteLine("Text received : {0}", data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    socket.Send(msg);
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        static void Main(string[] args)
        {
            start();
            //TcpListener server = null;
            //try
            //{
            //    // Set the TcpListener on port 13000.
            //    Int32 port = 13000;
            //    IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            //    // TcpListener server = new TcpListener(port);
            //    server = new TcpListener(localAddr, port);

            //    // Start listening for client requests.
            //    server.Start();


            //    // Enter the listening loop.
            //    while (true)
            //    {
            //        Console.Write("Waiting for a connection... ");

            //        // Perform a blocking call to accept requests.
            //        // You could also user server.AcceptSocket() here.
            //        TcpClient client = server.AcceptTcpClient();
            //        Console.WriteLine("Connected!");
            //        ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
            //        Thread clientThread = new Thread(ts);
            //        clientThread.Start(client);


            //    }
            //}
            //catch (SocketException e)
            //{
            //    Console.WriteLine("SocketException: {0}", e);
            //}
            //finally
            //{
            //    // Stop listening for new clients.
            //    server.Stop();
            //}


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public static void Worker(Object o)
        {
            TcpClient client = (TcpClient)o;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                data = data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", data);
            }

            // Shutdown and end connection
            client.Close();
        }


        

    }
}

