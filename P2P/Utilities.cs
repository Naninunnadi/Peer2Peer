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
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("wireless"))
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

        public static void ParseRequest(string user)
        {
            var request = new Request();
            request.Type =  new Uri(user).Segments.LastOrDefault();
            request.Name = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("name");
            request.Sendip = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("sendip");
            request.Sendport = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("sendport");
            request.TimeToLive = (string)HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("ttl");
            request.Id = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("id");
            request.Noask = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("noask").Split(new char[] { '_' }).ToList();

        }

        public static string SendRequest(Request request, string sendIp, string sendPort)
        {
            var url= "";
        //
        //    url = "http://"+sendIp+":"+sendPort+"/searchfile?"+request.Name+""
            return url;
        }
    }
}
