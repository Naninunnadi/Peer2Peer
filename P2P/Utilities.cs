using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace P2P
{
    public class Utilities
    {

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public static int LocalPort()
        {
            return 1234;
        }

        public static bool FindFile(string fileName)
        {
            string curFile = @"c:\wazaa\" + fileName;
            return File.Exists(curFile);
        }

        public static void ParseRequest(string request)
        {
            var name = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("name");
            var sendip = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("sendip");
            var sendport = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("sendport");
            var ttl = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("ttl");
            var id = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("id");
            var noask = HttpUtility.ParseQueryString(request.Substring(new[] { 0, request.IndexOf('?') }.Max())).Get("noask");
        }
    }
}
