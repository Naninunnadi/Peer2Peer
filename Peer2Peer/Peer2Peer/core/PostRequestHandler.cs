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

                //int testPort = Int32.Parse(SendPort)+1;
                //string replace = testPort.ToString();
                // this is where we will send it
                string uri = "http://" + SendIp + ":" + SendPort + "/foundfile?";

                // create a request
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(uri);
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                // turn our request string into a byte stream
                byte[] postBytes = Encoding.ASCII.GetBytes(Jsonlist);

                // this is important - make sure you specify type this way
                request.ContentType = "application/json"; //pm yolo application/json voib ka
                request.ContentLength = postBytes.Length;
                request.Timeout = 1000;
                request.KeepAlive = false;
                try
                {
                    Stream requestStream = request.GetRequestStream();

                    // now send it
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                    Console.WriteLine("Post Request Client: Sending to server: " + Jsonlist);


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Post Request Client: PostQuery failed, unable to connecto to SERVER");
                   
                }
                // grab te response and print it out to the console along with the status code
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    Console.Write("Post Request Client: Server Sent Statuscode: ");
                    Console.WriteLine(response.StatusCode);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Post Request Client: Request sent, but NO OK RESPONSE FROM SERVER");

                }
                
            
        }
    }
}
