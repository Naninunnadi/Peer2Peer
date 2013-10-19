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

        public static void doRequest()
        {
           var requestModel =  Utilities.ParseRequest("http://11.22.33.44:2345/searchfile?name=filename&sendip=55.66.77.88&sendport=6788&ttl=5&id=wqeqwe23&noask=11.22.33.44_111.222.333.444");
                //Our getVars, to test the get of our php. 
                //We can get a page without any of these vars too though.
                var getVars = "?name=" + requestModel.Name + "&sendip=" + requestModel.Sendip + "&sendport=" + requestModel.Sendport + "&ttl=5&id=wqeqwe23&noask=" + string.Join("_",requestModel.Noask);
                String operation = "searchfile";
                String IP = "http://"+Utilities.LocalIPAddress()+"/?";
                //IP = @IO.readFile("IP.txt");
                //Initialization, we use localhost, change if applicable

                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create
                    (String.Format("{2}"+"{0}"+"{1}",operation, getVars, IP));
                //This time, our method is GET.
                WebReq.Method = "GET";
            Console.WriteLine("RQ DONE");

            }
       
    }

}
