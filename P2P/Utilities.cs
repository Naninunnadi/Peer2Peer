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
            string curFile = @"c:\wazaa\" + fileName;
            return File.Exists(curFile);
        }

        public static void ParseRequest(string user)
        {
            var request = new Request();
            request.Name = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("name");
            request.Sendip = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("sendip");
            request.Sendport = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("sendport");
            request.TimeToLive = HttpUtility.ParseQueryString(user.Substring(new[] { 0, user.IndexOf('?') }.Max())).Get("ttl");
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
