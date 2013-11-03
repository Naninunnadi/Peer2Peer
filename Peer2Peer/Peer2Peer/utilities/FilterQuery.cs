using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Peer2Peer.utilities
{
    class FilterQuery
    {

        public static String getUrlParameterValues(String url)
        {

            //url = getAllParametersFromGetRequest(url);
            Console.WriteLine("VAATAN URI" + url);

            var query = HttpUtility.ParseQueryString(url);
            var var2 = query.Get("var2");
            var var1 = query.Get("var1");
            StringBuilder sr = new StringBuilder();
            sr.AppendLine(var2);
            sr.AppendLine(var1);
            //TODO dynaamiliseks
            //List<string> list = new List<string>();
            //foreach (String item in query.AllKeys)
            //{
            //  list.Add(query[item]);
            // Console.WriteLine(query[item]);
            //}
            //var var2 = query.Get("var2");
            return sr.ToString();
        }


        /// <summary>
        /// Finds operation from get request: GET xxx.com/?operation? > has to be between 2x "?"
        /// </summary>
        /// <param name="data">String full Get request</param>
        /// <returns>operation of get request</returns>
        public static String getOperationFromGetRequest(String data)
        {
            int index = data.IndexOf("?");
            int indexLast = data.LastIndexOf("?");
            //viimast küsimärki ei taha
            int length = (indexLast - index) - 1;
            return data.Substring(index + 1, length);
        }

        public static String getMainParamaterFromGetRequest(String data)
        {
            int index = data.LastIndexOf("?");
            //last HTTP1.1 > -2
            int indexLineBreak = data.IndexOf("H") - 2;
            int length = indexLineBreak - index;
            return data.Substring(index + 1, length);
        }

        public static String getMainParamaterFromGetRequestWithoutEquals(String data)
        {
            int index = data.IndexOf("=");
            //last HTTP1.1 > -2
            int indexLineBreak = data.IndexOf("H") - 2;
            int length = indexLineBreak - index;
            String rtrn = data.Substring(index + 1, length);
            return rtrn;
        }
    }
}
