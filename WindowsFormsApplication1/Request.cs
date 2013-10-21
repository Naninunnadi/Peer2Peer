using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Request
    {
        public RequestModel RequestModel { get; set; }
        public string SendIp  { get; set; }
        public string SendPort { get; set; }
        public RichTextBox RichTextBox { get; set; }

        public Request(RequestModel request, string ip, string port, RichTextBox textBox)
        {
            this.RequestModel = request;
            this.SendIp = ip;
            this.SendPort = port;
            this.RichTextBox = textBox;
        }

        public void doRequest()
        {
           //SetText1("jama");
            //Our getVars, to test the get of our php. 
            //We can get a page without any of these vars too though.
            var getVars = "http://" + SendIp + ":" + SendPort + "/searchfile?name=" + RequestModel.Name + "&sendip=" + RequestModel.Sendip + "&sendport=" + RequestModel.Sendport + "&ttl=" + RequestModel.TimeToLive + "&id=wqeqwe23&noask=" + string.Join("_", RequestModel.Noask);
            //String operation = "searchfile";
            //String IP = @"192.168.1.77:2234/?";
            //IP = @IO.readFile("IP.txt");
            //Initialization, we use localhost, change if applicable
            Uri targetUri = new Uri(getVars);
            Console.WriteLine("Client : " +targetUri.ToString());
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
            WebReq.Timeout = 10000;
            //This time, our method is GET.
            WebReq.Method = "GET";

            Console.WriteLine("Client : Request DONE");
            try
            {
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                
                //Let's show some information about the response
                Console.WriteLine("Client : Response from server >>> ");
                Console.WriteLine("Client: From Server: "+WebResp.StatusCode+" >GOT IT< ");
                Console.WriteLine("Client: From Server(what i sent to server (for debugging)): "+WebResp.ResponseUri);
            }
            catch (Exception e)
            {
                
                Console.WriteLine("Client: Request failed (Time-Out > Peer appears to be offline)");
            }
            
            Console.WriteLine("Request: I will quit. (QUERY SUCCEEDED");

            //Now, we read the response (the string), and output it.

        }
        delegate void SetTextCallback(string text);
        private void SetText1(string text)
        {
            if (this.RichTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                RichTextBox.Invoke(d, new object[] { text });
            }
            else
            {
                this.RichTextBox.Text = text;
            }
        }

    }

}
