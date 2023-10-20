using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public class OnlineClient
    {
        public Socket socket { get; set; }
        public OnlineUser user { get; set; }
    }
}
