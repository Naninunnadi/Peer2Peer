using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Peer2Peer.core
{
    public class DownloadManager
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }

        public DownloadManager(string ip, string port, string fileName)
        {
            this.Ip = ip;
            this.Port = port;
            this.Filename = fileName;

        }

        public DownloadManager(string path)
        {
            this.Path = path;
        }

        public static void aHttpResponse (String fname)
        {


            

        }



        public static void SendFile(string path, string IP)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            
            byte[] buffer = new byte[fs.Length];
            int len = (int)fs.Length;
            fs.Read(buffer, 0, len);
            fs.Close();


            BinaryFormatter br = new BinaryFormatter();
            TcpClient myclient = new TcpClient(IP, 1095);
            NetworkStream myns = myclient.GetStream();
            br.Serialize(myns, path); //fname
            

            BinaryWriter mysw = new BinaryWriter(myns);
            mysw.Write(buffer);
            mysw.Close();

            myns.Close();
            myclient.Close();
        }

        public static void ListenForFile(string pathAndFname)
        {
            NetworkStream myns;
            TcpListener mytcpl;
            Socket mysocket;
            BinaryReader bb;

            try
            {
                mytcpl = new TcpListener(1095);
                mytcpl.Start();
                mysocket = mytcpl.AcceptSocket();
                myns = new NetworkStream(mysocket);
                BinaryFormatter br = new BinaryFormatter();
                object op;

                op = br.Deserialize(myns); // Deserialize the Object from Stream


                bb = new BinaryReader(myns);
                byte[] buffer = bb.ReadBytes(1024*1024*2); //2mb

                FileStream fss = new FileStream(pathAndFname, FileMode.Create, FileAccess.Write);
                fss.Write(buffer, 0, buffer.Length);
                fss.Close();
                mytcpl.Stop();
            }
            catch (Exception ioException)
            {
                Console.WriteLine(ioException);
            }
            
        }
        
        public void doRequestForGetFile()
        {

            var getVars = "http://" + Ip + ":" + Port + "/getfile?fullname=" + Filename;
            Uri targetUri = new Uri(getVars);
            Console.WriteLine("DL Client >>> File Request : " + targetUri.ToString());
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
            WebReq.Timeout = 10000;
            WebReq.Method = "GET";
            WebReq.KeepAlive = false;
            Console.WriteLine("DL Client : File Request DONE");
            try
            {
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Console.WriteLine("DL Client : Response from peer >>> ");
                Console.WriteLine("DL Client: From Peer: " + WebResp.StatusCode + " >GOT Your GET request File< ");
                //Console.WriteLine("DL Client: From Server(what i sent to server (for debugging)): " + WebResp.ResponseUri);
                WebResp.Close();
            }
            catch (Exception e)
            {

                Console.WriteLine("Client: File Request failed (Time-Out > Peer appears to be offline)");
            }

            Console.WriteLine("File Request: I will quit. (QUERY OK!!)");

        }
    }
}
