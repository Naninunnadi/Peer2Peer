using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Request
    {
            public static void doRequest( RequestModel requestModel)
            {
                //Our getVars, to test the get of our php. 
                //We can get a page without any of these vars too though.
                String getVars = "?name=" + requestModel.Name + "&sendip=" + requestModel.Sendip + "&sendport=" + requestModel.Sendport + "&ttl=5&id=wqeqwe23&noask=" + string.Join("_",requestModel.Noask);
                String operation = "searchfile";
                String IP = "http://"+Utilities.LocalIPAddress()+"/?";

                //IP = @IO.readFile("IP.txt");

                //Initialization, we use localhost, change if applicable
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create
                    (String.Format("{2}"+"{0}"+"{1}",operation, getVars, IP));
                //This time, our method is GET.
                WebReq.Method = "GET";
                //From here on, it's all the same as above.
                
            }
       
    }
     public abstract class doSomething()

{
}
}
