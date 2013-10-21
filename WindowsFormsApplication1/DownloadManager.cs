﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class DownloadManager
    {


        //1095 jääb igal pool Download ja Upload static pordiks
        public static void SendFile(string path, string IP)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(IP, 1095);

                using (NetworkStream networkStream = client.GetStream())
                using (FileStream fileStream = File.OpenRead(path))
                {
                    ASCIIEncoding asci = new ASCIIEncoding();
                    byte[] b = asci.GetBytes(path);
                    networkStream.Write(b, 0, b.Length);
                    networkStream.Flush();
                    fileStream.CopyTo(networkStream);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("DLMGR: Error sending file");
            }
            client.Close();
        }


       public static void ListenForFile(string pathAndFname)// c:/wazaa/nimi.txt
        {
            
           
           TcpListener listener = new TcpListener(IPAddress.Any, 1095);
            listener.Start();
           Console.WriteLine("DLMGR: Listening....");
            while (true)
            {
                try
                {
                    using (TcpClient incoming = listener.AcceptTcpClient())
                    using (NetworkStream networkStream = incoming.GetStream())
                    using (FileStream fileStream = File.OpenWrite(pathAndFname))
                    {
                        networkStream.CopyTo(fileStream);
                        break; //järgmise faili jaoks
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("DLMGR: Error saving file");
                    break;
                }
            }
            listener.Stop();
            Console.WriteLine("DLMGR: Download mngr stopped listening for incoming connections");
           
        }

        //http://11.22.33.44:2345/getfile?fullname=fullfilename?askerip=
       public void doRequestForGetFile()
       {
           
           var getVars = "http://" + SendIp + ":" + SendPort + "/searchfile?name=" + RequestModel.Name + "&sendip=" + RequestModel.Sendip + "&sendport=" + RequestModel.Sendport + "&ttl=" + RequestModel.TimeToLive + "&id=wqeqwe23&noask=" + string.Join("_", RequestModel.Noask);
           Uri targetUri = new Uri(getVars);
           Console.WriteLine("Client : " + targetUri.ToString());
           HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
           WebReq.Timeout = 6000;
           WebReq.Method = "GET";

           Console.WriteLine("Client : Request DONE");
           try
           {
               HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

               //Let's show some information about the response
               Console.WriteLine("Client : Response from server >>> ");
               Console.WriteLine("Client: From Server: " + WebResp.StatusCode + " >GOT IT< ");
               Console.WriteLine("Client: From Server(what i sent to server (for debugging)): " + WebResp.ResponseUri);
           }
           catch (Exception e)
           {

               Console.WriteLine("Client: Request failed (Time-Out > Peer appears to be offline)");
           }

           Console.WriteLine("Request: I will quit. (QUERY SUCCEEDED");

           //Now, we read the response (the string), and output it.

       }
    }
}
