using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Peer2Peer.core
{
    public class DownloadManager
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Filename { get; set; }

        public DownloadManager(string ip, string port, string fileName)
        {
            this.Ip = ip;
            this.Port = port;
            this.Filename = fileName;


        }

        //1095 jääb igal pool Download ja Upload static pordiks
        public static void SendFile(string path, string IP)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(IP, 1095);
                Console.WriteLine("DLMGR: Sending file to > {0}", IP);
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
            catch (Exception)
            {
                Console.WriteLine("DLMGR: Error sending file");
            }
            client.Close();
        }


        public static void ListenForFile(string pathAndFname) // c:/wazaa/nimi.txt
        {


            TcpListener listener = new TcpListener(IPAddress.Any, 1095);
            listener.Start();
            Console.WriteLine("DLMGR: Listening....");

            {
                try
                {
                    using (TcpClient incoming = listener.AcceptTcpClient())
                    using (NetworkStream networkStream = incoming.GetStream())
                    using (FileStream fileStream = File.OpenWrite(pathAndFname))
                    {
                        networkStream.CopyTo(fileStream);

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("DLMGR: Error saving file");

                }

                listener.Stop();
                Console.WriteLine("DLMGR: Download mngr stopped listening for incoming connections");

            }
        }

        public void doRequestForGetFile()
        {

            var getVars = "http://" + Ip + ":" + Port + "/getfile?fullname=" + Filename;
            Uri targetUri = new Uri(getVars);
            Console.WriteLine("DL Client >>> File Request : " + targetUri.ToString());
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
            WebReq.Timeout = 1000;
            WebReq.Method = "GET";
            WebReq.KeepAlive = false;

            Console.WriteLine("DL Client : File Request DONE");
            try
            {
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

                //Let's show some information about the response
                Console.WriteLine("DL Client : Response from server >>> ");
                Console.WriteLine("DL Client: From Server: " + WebResp.StatusCode + " >GOT IT< ");
                Console.WriteLine("DL Client: From Server(what i sent to server (for debugging)): " + WebResp.ResponseUri);
            }
            catch (Exception e)
            {

                Console.WriteLine("Client: File Request failed (Time-Out > Peer appears to be offline)");
            }

            Console.WriteLine("File Request: I will quit. (QUERY SUCCEEDED");

        }
    }
}
