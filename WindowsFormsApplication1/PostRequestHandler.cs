using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class PostRequestHandler
    {
       
        public static void runRequest()
        {
            //fthis
        }
        
        
        public static void postRequest()
        {

            while (true)
            {

                // this is where we will send it
                string uri = "http://192.168.5.185:2234/foundfile?";

                // create a request
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(uri);
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                // turn our request string into a byte stream
                String json = "s=siiiiiiiiiiiiiiiiaaaaaaaaaaaaaa";
                byte[] postBytes = Encoding.ASCII.GetBytes(json);

                // this is important - make sure you specify type this way
                request.ContentType = "application/json"; //pm yolo application/json voib ka
                request.ContentLength = postBytes.Length;
                try
                {
                    Stream requestStream = request.GetRequestStream();

                    // now send it
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                    Console.WriteLine("Client: Sending to server: " + json);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Client: PostQuery failed, unable to connecto to SERVER");
                    break;
                }
                // grab te response and print it out to the console along with the status code
                try
                {
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    Console.Write("Server Sent Statuscode: ");
                    Console.WriteLine(response.StatusCode);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Client: Request sent, but NO OK RESPONSE FROM SERVER");
                    
                }
                break;
            }
        }
    }
}
