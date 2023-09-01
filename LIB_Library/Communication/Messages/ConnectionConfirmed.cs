using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Messages
{
    [Serializable]
    public class ConnectionConfirmed : MessageBase
    {
        public int TestCode;
        public ConnectionConfirmed(int code) 
            : base(CommunicationCnst.Messages.ConnectionConfirmed, CommunicationCnst.MessageType.HostToCliet)
        {
            TestCode = code;
        }
    }
}
