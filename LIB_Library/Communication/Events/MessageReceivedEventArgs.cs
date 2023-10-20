using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public object MessageReceived { get; set; }

        public MessageReceivedEventArgs(object messageReceived)
        {
            MessageReceived = messageReceived;
        }
    }
}
