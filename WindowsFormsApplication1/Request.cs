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
        public string RequestString { get; set; }

        public Request(string request)
        {
            this.RequestString = request;
        }

        public void doRequest()
        {
           var requestModel =  Utilities.ParseRequest("http://192.168.1.77:2234/searchfile?name=filename&sendip=55.66.77.88&sendport=6788&ttl=5&id=wqeqwe23&noask=11.22.33.44_111.222.333.444");
                //Our getVars, to test the get of our php. 
                //We can get a page without any of these vars too though.
                var getVars = "?name=" + requestModel.Name + "&sendip=" + requestModel.Sendip + "&sendport=" + requestModel.Sendport + "&ttl=5&id=wqeqwe23&noask=" + string.Join("_",requestModel.Noask);
                //String operation = "searchfile";
                //String IP = @"192.168.1.77:2234/?";
                //IP = @IO.readFile("IP.txt");
                //Initialization, we use localhost, change if applicable
                Uri targetUri = new Uri("http://192.168.1.77:2234/test?q=yolo");
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
