﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;
using Newtonsoft.Json;
using Peer2Peer.utilities;

namespace Peer2Peer.core
{
    internal class TcpServer
    {
        public FlowLayoutPanel FlowLayoutPanel { get; set; }
        public static string text { get; set; }
        public static string ip { get; set; }
        public static TcpClient client { get; set; }

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
                Byte[] bytes = new Byte[1024 * 1024];
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

                    if (data2.ToUpper().Contains("GET") && data2.ToUpper().Contains("ALIVE") && !data2.ToUpper().Contains("ICO"))
                    {
                        var fName = FilterQuery.getMainParamaterFromGetRequestWithoutEquals(data2);
                        var fNameAndPath = @"D:\wazaa\" + fName;
                        if (Directory.Exists(@"C:\"))
                        {
                            fNameAndPath = @"C:\wazaa\" + fName;
                        }
                        Console.WriteLine("Server: Got from browser file request. Name: {0} -- AndPath: {1}", fName, fNameAndPath);
                        var response = DownloadManager.convertToHttpResponse(fNameAndPath); //saadame failinime, et seal ehitada response
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                        //Image image = Image.FromFile(fNameAndPath);
                        //MemoryStream ms = new MemoryStream();
                        //image.Save(ms, image.RawFormat);
                        //ms.Close();
                        //byte[] responseContent = ms.ToArray();
                        //stream.Write(responseContent, 0, responseContent.Length);
                        int iTotBytes = 0;
                        string sResponse = "";
                        FileStream fs = new FileStream(fNameAndPath,
                                        FileMode.Open, FileAccess.Read,
                          FileShare.Read);
                        BinaryReader reader = new BinaryReader(fs);
                        byte[] bytes2 = new byte[fs.Length];
                        int read;
                        while ((read = reader.Read(bytes2, 0, bytes2.Length)) != 0)
                        {
                            sResponse = sResponse + Encoding.ASCII.GetString(bytes2, 0, read);
                            iTotBytes = iTotBytes + read;

                        }
                        reader.Close();
                        fs.Close();
                        stream.Write(bytes2, 0, bytes2.Length);
                        goto ShutDown;
                    }

                    if (data2.ToUpper().Contains("FAVICON")) { goto ShutDown; }

                    if (!secondCycle) data = "HTTP/1.1 200 OK\nContent-Type: text/plain\nConnection: Close\n\n0";

                    if (!secondCycle)
                    {
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Server with IP: {1}:{2} - has Sent this: {0}", data, getip, getport);
                    }

                    if (data2.ToUpper().Contains("POST")) 
                    {
                        secondCycle = true;
                        stream.Flush();
                        goto redo;
                    }

                    if (!data2.ToUpper().Contains("GET"))
                    {
                        SetText1(data2, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());


                    }

                    

                    if (data2.ToUpper().Contains("GET") && data2.Contains("fullname"))
                    {

                        Console.WriteLine("Server: GOT DOWNLOAD REQUEST FROM ASKER");
                        if (Directory.Exists(@"C:\"))
                        {
                            var param1 = FilterQuery.getMainParamaterFromGetRequestWithoutEquals(data2);
                            DownloadManager.SendFile(@"C:\wazaa\" + param1, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                        }
                        else if (Directory.Exists(@"D:\"))
                        {
                            var param1 = FilterQuery.getMainParamaterFromGetRequestWithoutEquals(data2);
                            DownloadManager.SendFile(@"D:\wazaa\" + param1, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                        }

                    }

                    

                    if (data2.ToUpper().Contains("GET") && !data2.Contains("fullname"))
                    {

                        utilities.Factory.filterAndDistributeQuery(data2);

                    }

                    break;
                }
            ShutDown:
                Console.WriteLine("Server: Shutdown and end connection");

                Console.WriteLine("Server: Initalizing for new incoming requests........");
                stream.Close();
                client.Close();
                goto ListeningLoop;
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server: SocketException: {0}", e);
            }
            finally
            {
                
                server.Stop();
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
            worker.Name = "RequestFileTHREAD";
            worker.Start();


            
            if (Directory.Exists(@"C:\"))
            {
                DownloadManager.ListenForFile(@"C:\wazaa\" + btn.Text);
            }
            else if (Directory.Exists(@"D:\"))
            {
                DownloadManager.ListenForFile(@"D:\wazaa\" + btn.Text);
            }

        }

      
    }
}

