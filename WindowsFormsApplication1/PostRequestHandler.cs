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
            
        }
        
        
        public static void postRequest()
        {
            HttpWebRequest httpWReq =
            (HttpWebRequest)WebRequest.Create("http://192.168.5.185:2234/");

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "username=user";
            postData += "&password=pass";
            byte[] data = encoding.GetBytes(postData);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/x-www-form-urlencoded";
            httpWReq.ContentLength = data.Length;

            try
            {

                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch(Exception e)
            {
                Console.WriteLine("SERVER: GOT no response for POST call");
            }


        }
    }
}
