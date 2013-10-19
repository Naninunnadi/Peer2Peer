using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            string curFile = @"c:\wazaa\" + fileName;
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

        //public static string SendRequest(Request request, string sendIp, string sendPort)
        //{
        //    var url = "";
        //    //
        //    //    url = "http://"+sendIp+":"+sendPort+"/searchfile?"+request.Name+""
        //    return url;
        //}
       
    }
}
