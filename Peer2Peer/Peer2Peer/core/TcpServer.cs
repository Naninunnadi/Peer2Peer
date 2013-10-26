using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Peer2Peer.utilities;

namespace Peer2Peer.core
{
    internal class TcpServer
    {
        public FlowLayoutPanel FlowLayoutPanel { get; set; }
        public static string text { get; set; }

        public TcpServer(FlowLayoutPanel flowLayoutPanel)
        {
            this.FlowLayoutPanel = flowLayoutPanel;

        }

        public void TcpServerListen()
        {
        
            TcpListener server = null;
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
                Byte[] bytes = new Byte[1024 * 256];
                String data = null;

                    Console.WriteLine("Server: Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Server: Connected!");
                    NetworkStream stream = client.GetStream();
                    Console.WriteLine("**********************************************************");
                    Console.WriteLine("A connection from: {0} - Port: {1}", client.Client.RemoteEndPoint.ToString(),
                                      ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
                    Console.WriteLine("Has been establisehd to this server: {0} - Port: {1} ",
                                      client.Client.LocalEndPoint.ToString(),
                                      ((IPEndPoint)client.Client.LocalEndPoint).Port.ToString());
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

                        if (!data2.ToUpper().Contains("GET"))
                        {
                            SetText1(data2, ((IPEndPoint)client.Client.LocalEndPoint).Address.ToString());
                            client.Close();
                            goto ListeningLoop;
                        }
                        if (data2.ToUpper().Contains("GET") && data2.Contains("fullname"))
                        {

                            Console.WriteLine("Server: GOT DOWNLOAD REQUEST FROM ASKER");
                            if (Directory.Exists(@"C:\"))
                            {
                                //DownloadManager.SendFile(@"C:\wazaa\");
                                client.Close();
                                goto ListeningLoop;
                                //TCP listener saab get query ja stardib /getfile=fname ja IP kuhu saata ja saadab wazaa kaustast faili
                            }
                            else if (Directory.Exists(@"D:\"))
                            {
                                client.Close();
                                goto ListeningLoop;
                            }
                            client.Close();
                            goto ListeningLoop;
                            
                        }

                        if (data2.ToUpper().Contains("GET") && !data2.Contains("fullname"))
                        {
                            
                            utilities.Factory.filterAndDistributeQuery(data2);
                            client.Close();
                            goto ListeningLoop;
                        }
                        

                    }
                    Console.WriteLine("Server: Shutdown and end connection");
                    
                    Console.WriteLine("Server: Initalizing for new incoming requests........");

                    client.Close();
                    goto ListeningLoop;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server: SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                
                server.Stop();
                //Console.WriteLine("server: FINAL SERVER STOP (E X C P E C T E D)");
                
                Console.WriteLine("server: FINAL SERVER STOP (unexpected)");
            }
            
        }

        private delegate void SetTextCallback(string text, string ipAndPort);

        private void SetText1(string text, string ipAndPort)
        {
            if (this.FlowLayoutPanel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                FlowLayoutPanel.Invoke(d, new object[] { text, ipAndPort });
            }
            else
            {
                var mydata = JsonConvert.DeserializeObject<PostObject>(text);
                Label label = new Label();
                label.Text = ipAndPort;
                FlowLayoutPanel.Height = FlowLayoutPanel.Height + 70;
                label.AutoSize = true;
                FlowLayoutPanel.Controls.Add(label);
                foreach (var item in mydata.Files)
                {
                    Button button = new Button();
                    button.AutoSize = true;
                    button.Click += new System.EventHandler(GetFile);
                    button.Tag = item.Ip + ":" + item.Port;
                    button.Text = item.Name;
                    FlowLayoutPanel.Controls.Add(button);
                }
            }
        }
        private void GetFile(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var ipandPort = btn.Tag.ToString().Split(':');
            Thread worker = new Thread(new DownloadManager(ipandPort[0], ipandPort[1], btn.Text).doRequestForGetFile);
            worker.IsBackground = true;
            worker.SetApartmentState(System.Threading.ApartmentState.STA);
            worker.Name = "TCPLISTENERTHREAD";
            worker.Start();
            Thread.Sleep(1);

            DownloadManager.ListenForFile(@"c:\wazaa\" + btn.Text);


        }
    }
}

