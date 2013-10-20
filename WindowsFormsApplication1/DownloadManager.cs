using System;
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


       public static void ListenForFile(string pathAndFname)
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
    }
}
