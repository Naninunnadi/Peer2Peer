using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class TcpListenerServer 
    {
        public static void test()
        {
            
            TcpListener server = null;
            StringBuilder stringBuilder = new StringBuilder(1024*8);

            try
            {
                // Set the TcpListener on port 13000.
                String getip = Utilities.LocalIPAddress();
                String getport = Utilities.LocalPort();
                Int32 port = Int32.Parse(getport);
                IPAddress localAddr = IPAddress.Parse(getip); //192.168.5.143
                
                // TcpListener server = new TcpListener(port);
                Console.WriteLine("Server: listening now to: "+getip+":"+getport);
                server = new TcpListener(localAddr, port);
                
                // Start listening for client requests.
                server.Start();
                
                // Buffer for reading data
                Byte[] bytes = new Byte[10000];
                String data = null;
                ListeningLoop:
                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Server: Waiting for a connection... ");
                    

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Server: Connected!");

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

                        //todo siit saadame filtrisse
                        //Console.WriteLine(FilterQuery.getOperationFromGetRequest(data));
                        //FilterQuery.getAllParametersFromGetRequest(data);
                        // Process the data sent by the client.
                        data = "HTTP/1.1 200 OK\nContent-Type: text/plain\n\nstatus=gotit";
                        
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);

                        Console.WriteLine("Serevr has Sent: {0}", data);
                        break;

                    }
                    Console.WriteLine("Server: Shutdown and end connection");
                    client.Close();
                    goto ListeningLoop;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server: SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                Console.WriteLine("server: FINAL SERVER S>Top");
            }
        }


        
    }
}

