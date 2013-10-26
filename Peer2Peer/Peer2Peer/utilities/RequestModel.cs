using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peer2Peer.utilities
{
    public class RequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sendip { get; set; }
        public string Sendport { get; set; }
        public string TimeToLive { get; set; }
        public List<string> Noask { get; set; }

    }
}
