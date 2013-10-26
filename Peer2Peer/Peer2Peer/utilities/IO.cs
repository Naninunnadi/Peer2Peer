using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peer2Peer.utilities
{
    class IO
    {
        public static String readFile(String name)
        {
            String result = string.Empty;
            if (name != null)
            {
                StreamReader file = new StreamReader(name);
                result = file.ReadLine();
            }
            return result;
        }
    }
}
