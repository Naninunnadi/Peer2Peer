using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peer2Peer.utilities;

namespace Peer2Peer.core
{
    class GetRequestHandler
   
    {
        public RequestModel RequestModel { get; set; }
        public string SendIp  { get; set; }
        public string SendPort { get; set; }
        public RichTextBox RichTextBox { get; set; }

        public GetRequestHandler(RequestModel request, string ip, string port) 
        {
            this.RequestModel = request;
            this.SendIp = ip;
            this.SendPort = port;
        }

        public void doRequest()
        {
           
            var getVars = "http://" + SendIp + ":" + SendPort + "/searchfile?name=" + RequestModel.Name + "&sendip=" + RequestModel.Sendip + "&sendport=" + RequestModel.Sendport + "&ttl=" + RequestModel.TimeToLive + "&id=wqeqwe23&noask=" + string.Join("_", RequestModel.Noask);
            Uri targetUri = new Uri(getVars);
            Console.WriteLine("Client : " +targetUri.ToString());
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(targetUri);
            WebReq.Timeout = 10000;
            WebReq.Method = "GET";
            WebReq.KeepAlive = false;
            Console.WriteLine("Client : GET Request DONE");
            
            try
            {
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
               
                Console.WriteLine("Client: For Get req > Response from peer >>> ");
                Console.WriteLine("Client: From Peer: "+WebResp.StatusCode+" >GOT your GET query< ");
                //Console.WriteLine("Client: From PEER(what i sent to server (for debugging)): "+WebResp.ResponseUri);
                WebResp.Close();
            }
            catch (Exception e)
            {
                
                Console.WriteLine("Client: Request failed (Time-Out > Peer appears to be offline)" + e);
                
            }        
            Console.WriteLine("Request: I will quit. (QUERY SUCCEEDED");
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
