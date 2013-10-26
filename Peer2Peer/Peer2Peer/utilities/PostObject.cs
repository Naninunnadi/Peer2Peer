using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peer2Peer.utilities
{
    public class PostObject
    {
        public string Id { get; set; }
        public IList<FileModel> Files { get; set; }
    }
}
