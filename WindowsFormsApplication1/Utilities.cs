using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public class Utilities
    {

        public static string LocalIPAddress()
        {
            var ip = string.Empty;
            var re = System.IO.File.ReadLines("Config.txt").FirstOrDefault();
            if (re != null)
                ip = re.Split(null).FirstOrDefault();
            return ip;
        }

        public static string LocalPort()
        {
            var port = string.Empty;
            var re = System.IO.File.ReadLines("Config.txt").FirstOrDefault();
            if (re != null)
                port = re.Split(null).LastOrDefault();
            return port;
        }

        public static bool FindFile(string fileName)
        {


            string curFile = string.Empty;
            if (Directory.Exists(@"C:\wazaa"))
            {
                curFile = @"C:\wazaa\" + fileName;
                return File.Exists(curFile);
            }


            curFile = @"D:\wazaa\" + fileName;
            return File.Exists(curFile);

        }

        public static RequestModel ParseRequest(string reuquestString)
        {
            var request = new RequestModel();
            request.Name =
                HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("name");
            request.Sendip = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("sendip");
            request.Sendport = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("sendport");
            request.TimeToLive =
                HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("ttl");
            request.Id =
                HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("id");
            request.Noask =
                HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max()))
                    .Get("noask")
                    .Split(new char[] { '_' })
                    .ToList();
            return request;
        }

        public static List<KeyValuePair<IPAddress, string>> getIpsAndPorts()
        {
            List<KeyValuePair<IPAddress, string>> clientList = new List<KeyValuePair<IPAddress, string>>();
            string[] clients = System.IO.File.ReadAllLines("IP&Port.txt");
            var sth = JArray.Parse(File.ReadAllText("IP&Port.txt"));

            foreach (var client in sth.ToList())
            {
                IPAddress Ip = null;
                string port = "";
                if (!string.IsNullOrEmpty(client.First.Value<string>()))
                    Ip = IPAddress.Parse(client.First.Value<string>());
                if (!string.IsNullOrEmpty(client.Last.Value<string>()))
                    port = client.Last.Value<string>();
                if (Ip != null && !string.IsNullOrEmpty(port))
                    clientList.Add(new KeyValuePair<IPAddress, string>(Ip, port));
            }
            return clientList;
        }

        public static void dosomething(string data)
        {
            var startParsing = Utilities.ParseRequest(FilterQuery.getAllParametersFromGetRequest(data));
            var ttl = Int32.Parse(startParsing.TimeToLive)-1;
            if (!ServerUtilitiesOnly.FindFile(startParsing.Name) && ttl>0)
            {
                var places = Utilities.getIpsAndPorts();
                foreach (var keyValuePair in places)
                {
                    if (startParsing.Noask.Contains(keyValuePair.Key.ToString()))
                        return;
                    var requestModel = startParsing;
                    requestModel.TimeToLive = ttl.ToString();
                    requestModel.Noask.Add(LocalIPAddress());
                    var request = new Request(requestModel, keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
                    Thread worker = new Thread(request.doRequest);
                    worker.IsBackground = true;
                    worker.SetApartmentState(System.Threading.ApartmentState.STA);
                    worker.Start();
                }
            }
        }



    }
}
