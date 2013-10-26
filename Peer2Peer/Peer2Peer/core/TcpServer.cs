using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peer2Peer.core
{
    internal class TcpServer
    {
        //public RichTextBox RichTextBox { get; set; }

        //public TcpServer(RichTextBox textBox)
        //{
        //    this.RichTextBox = textBox;

        //}

        public static void TcpServerListen()
        {

            TcpListener server = null;
            StringBuilder stringBuilder = new StringBuilder(1024*8);

            try
            {
                String getip = utilities.Factory.LocalIPAddress();
                String getport = utilities.Factory.LocalPort();
                Int32 port = Int32.Parse(getport);
                IPAddress localAddr = IPAddress.Parse(getip); 
                Console.WriteLine("Server: Starting now to listen to: " + getip + ":" + getport);
                server = new TcpListener(localAddr, port);
                server.Start();
            ListeningLoop:
                Byte[] bytes = new Byte[10000];
                String data = null;

                while (true)
                {
                   
                    Console.Write("Server: Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Server: Connected!");
                    data = null;
                    NetworkStream stream = client.GetStream();
                    Console.WriteLine("**********************************************************");
                    Console.WriteLine("A connection from: {0} - Port: {1}", client.Client.RemoteEndPoint.ToString(),
                                      ((IPEndPoint) client.Client.RemoteEndPoint).Port.ToString());
                    Console.WriteLine("Has been establisehd to this server: {0} - Port: {1} ",
                                      client.Client.LocalEndPoint.ToString(),
                                      ((IPEndPoint) client.Client.LocalEndPoint).Port.ToString());
                    Console.WriteLine("**********************************************************");
                    bool secondCycle = false;
                redo:
                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("SERVER: Received: {0}", data);
                        String data2 = data;
                        if (!secondCycle) data = "HTTP/1.1 200 OK\nContent-Type: text/plain\n\n0";

                        if (!secondCycle)
                        {
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Server with IP: {1}:{2} - has Sent this: {0}", data, getip, getport);
                        }

                        if (data2.ToUpper().Contains("POST")) //teises ringis ei ole enam POST sees, läeb edasi
                        {
                            secondCycle = true;
                            goto redo;
                        }

                        if (!data2.ToUpper().Contains("GET")) Console.WriteLine("SERVER: Query doesnt contain GET");//SetText1(data2 + ";"); 

                        if (data2.ToUpper().Contains("GET") && data2.ToUpper().Contains("fullfilename"))
                        {

                            Console.WriteLine("Server: GOT DOWNLOAD REQUEST FROM ASKER");
                            if (Directory.Exists(@"C:\"))
                            {
                                //DownloadManager.SendFile(@"C:\wazaa\");
                                //TCP listener saab get query ja stardib /getfile=fname ja IP kuhu saata ja saadab wazaa kaustast faili
                            }
                            else if (Directory.Exists(@"D:\"))
                            {

                            }
                        }

                        if (data2.ToUpper().Contains("GET"))
                        {
                            utilities.Factory.filterAndDistributeQuery(data2);
                        }
                        break;

                    }
                    Console.WriteLine("Server: Shutdown and end connection");
                    client.Close();
                    Console.WriteLine("Server: Initalizing for new incoming requests........");

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
                Console.WriteLine("server: FINAL SERVER STOP (unexpected)");
            }
        }

        private delegate void SetTextCallback(string text);

        //private void SetText1(string text)
        //{
        //    if (this.RichTextBox.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetText1);
        //        RichTextBox.Invoke(d, new object[] {text});
        //    }
        //    else
        //    {
        //        this.RichTextBox.Text += (text);
        //    }
        //}
    }
}

