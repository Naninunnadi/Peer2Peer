using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class FilterQuery
    {
        /// <summary>
        /// Finds operation from get request: GET xxx.com/?operation? > has to be between 2x "?"
        /// </summary>
        /// <param name="data">String full Get request</param>
        /// <returns>operation of get request</returns>
        public static String getOperationFromGetRequest(String data)
        {
            int index = data.IndexOf("?");
            int indexLast = data.LastIndexOf("?");
            int length = indexLast - index;
            return data.Substring(index + 1, length);
        }
    }
}
