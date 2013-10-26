using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Peer2Peer.core;

namespace Peer2Peer.utilities
{
    public class Factory
    {
        public string PostJson { get; set; }
        public static string AllTheQues { get; set; }
        public RichTextBox RichTextBox {get; set; }

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

        public static List<string> FindFile(string fileName)
        {
            var stringList = new List<string>();
            string curFile = string.Empty;
            if (Directory.Exists(@"C:\wazaa"))
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(@"c:\wazaa\");
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + fileName + "*.*");

                foreach (FileInfo foundFile in filesInDir)
                {

                    stringList.Add(foundFile.Name);
                }
            }
            else if (Directory.Exists(@"D:\wazaa"))
            {
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(@"D:\wazaa\");
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + fileName + "*.*");

                foreach (FileInfo foundFile in filesInDir)
                {
                    string fullName = string.Format(foundFile.Directory + @"\" + foundFile.Name);
                    stringList.Add(foundFile.Name);
                }
            }

            return stringList;

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
            string[] clients = System.IO.File.ReadAllLines("Machines.txt");
            var sth = JArray.Parse(File.ReadAllText("Machines.txt"));

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

        public static void filterAndDistributeQuery(string data)
        {
            var startParsing = ParseRequest(FilterQuery.getAllParametersFromGetRequest(data));
            var ttl = Int32.Parse(startParsing.TimeToLive) - 1;
            var foundFiles = FindFile(startParsing.Name);
            var fileList = new List<FileModel>();
            if (!foundFiles.Any() && ttl > 0)
            {
                var places = getIpsAndPorts();
                foreach (var keyValuePair in places)
                {
                    if (startParsing.Noask != null && startParsing.Noask.Contains(keyValuePair.Key.ToString()))
                        return;
                    var requestModel = startParsing;
                    requestModel.TimeToLive = ttl.ToString();
                    requestModel.Noask.Add(LocalIPAddress());
                    var request = new GetRequestHandler(requestModel, keyValuePair.Key.ToString(), keyValuePair.Value.ToString());
                    Thread worker = new Thread(request.doRequest);
                    worker.IsBackground = true;
                    worker.SetApartmentState(System.Threading.ApartmentState.STA);
                    worker.Name = "ParseRequestsAndSendRequestsTTL";
                    worker.Start();
                }
            }
            else if (foundFiles.Any() && ttl > 0)
            {
                foreach (var file in foundFiles)
                {
                    fileList.Add(new FileModel { Ip = LocalIPAddress(), Port = LocalPort(), Name = file });
                }
                var json = JsonConvert.SerializeObject(new PostObject { Id = String.Empty, Files = fileList });
                var postreq = new PostRequestHandler(json, startParsing.Sendip, startParsing.Sendport);
                //postreq.postRequest();
                Thread worker = new Thread(postreq.postRequest);
                worker.IsBackground = true;
                worker.SetApartmentState(System.Threading.ApartmentState.STA);
                worker.Name = "PostRequestThreadHE";
                worker.Start();

            }
            else
            {
                //TODO:send error sest ttl on l'bi
            }
        }

        public bool IsThisGetOrPost(string data)
        {
            return !FilterQuery.getAllParametersFromGetRequest(data).Contains("GET");
        }

        
    }
}
