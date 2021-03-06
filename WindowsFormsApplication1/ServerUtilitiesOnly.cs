﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WindowsFormsApplication1
{
    class ServerUtilitiesOnly
    {

        public static Boolean dlManagerUp = true;

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
            if (Directory.Exists(@"D:\wazaa"))
            {
                curFile = @"D:\wazaa\" + fileName;
                return File.Exists(curFile);
            }

            return File.Exists(curFile);

        }

        public static RequestModel ParseRequest(string reuquestString)
        {
            var request = new RequestModel();
            request.Name = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("name");
            request.Sendip = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("sendip");
            request.Sendport = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("sendport");
            request.TimeToLive = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("ttl");
            request.Id = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("id");
            request.Noask = HttpUtility.ParseQueryString(reuquestString.Substring(new[] { 0, reuquestString.IndexOf('?') }.Max())).Get("noask").Split(new char[] { '_' }).ToList();
            return request;
        }

        public static List<KeyValuePair<IPAddress, int>> getIpsAndPorts()
        {
            List<KeyValuePair<IPAddress, int>> clientList = new List<KeyValuePair<IPAddress, int>>();
            string[] clients = System.IO.File.ReadAllLines("IP&Port.txt");

            foreach (var client in clients)
            {
                IPAddress iP = null;
                int port = 0;
                string[] pieces = client.Split(null);
                if (pieces.FirstOrDefault() != null)
                    iP = IPAddress.Parse(pieces.FirstOrDefault());
                if (pieces.LastOrDefault() != null)
                    int.TryParse(pieces.LastOrDefault(), out port);
                if (iP != null && port != 0)
                    clientList.Add(new KeyValuePair<IPAddress, int>(iP, port));
            }
            return clientList;
        }

    }
}
