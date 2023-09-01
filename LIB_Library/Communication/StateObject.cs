using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication
{
    public class StateObject
    {
        public Socket workSocket = null;
        public const int BUFFER_SIZE = 4096;
        public byte[] buffer = new byte[BUFFER_SIZE];
        //public StringBuilder sb = new StringBuilder();
    }
}
