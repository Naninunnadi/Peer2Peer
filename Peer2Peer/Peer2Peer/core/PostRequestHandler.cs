using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Peer2Peer.core
{
    class PostRequestHandler
    {
        public string Jsonlist { get; set; }
        public string SendIp { get; set; }
        public string SendPort { get; set; }


        public PostRequestHandler(string jsonlist, string sendIp, string sendPort)
        {
            this.Jsonlist = jsonlist;
            this.SendIp = sendIp;
            this.SendPort = sendPort;

        }


        public static void runRequest()
        {
            //fthis
        }


        public void postRequest()
        {

                
                string uri = "http://" + SendIp + ":" + SendPort + "/foundfile?";
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(uri);
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.KeepAlive = false;
                byte[] postBytes = Encoding.ASCII.GetBytes(Jsonlist);
                request.ContentType = "application/json"; 
                request.ContentLength = postBytes.Length;
                
                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                    Console.WriteLine("Post Request Client: Sending to peer ({1}:{2}): {0}",Jsonlist,SendIp,SendPort);
         
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Post Request Client: PostQuery failed, unable to connecto to SERVER");
                   
                }
        
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    Console.Write("Post Request Client: Server Sent Statuscode: ");
                    Console.WriteLine(response.StatusCode);
                    response.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Post Request Client: Request sent, but NO OK RESPONSE FROM SERVER");

                }
                
            
        }
    }
}
