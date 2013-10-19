using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Request
    {
        public RequestModel RequestModel { get; set; }

        public Request(RequestModel request)
        {
            this.RequestModel = request;
        }

        public void doRequest()
        {

            //Our getVars, to test the get of our php. 
            //We can get a page without any of these vars too though.
            var getVars = "?name=" + RequestModel.Name + "&sendip=" + RequestModel.Sendip + "&sendport=" + RequestModel.Sendport + "&ttl=5&id=wqeqwe23&noask=" + string.Join("_", RequestModel.Noask);
            //String operation = "searchfile";
            //String IP = @"192.168.1.77:2234/?";
            //IP = @IO.readFile("IP.txt");
            //Initialization, we use localhost, change if applicable
            Uri targetUri = new Uri(getVars);
            Console.WriteLine(targetUri.ToString());
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
            //This time, our method is GET.
            WebReq.Method = "GET";

            Console.WriteLine("RQ DONE");
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            //Let's show some information about the response
            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);
            Console.WriteLine(WebResp.ResponseUri);

            //Now, we read the response (the string), and output it.

        }

    }

}
