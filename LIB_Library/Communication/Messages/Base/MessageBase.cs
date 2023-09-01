using LIB.Communication.Constants;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Policy;

namespace LIB.Communication.Messages.Base
{
    [Serializable]
    public abstract class MessageBase
    {
        public IPAddress Sender;
        public int MessageCode;
        public CommunicationCnst.MessageType MessageType;

        public MessageBase(int messageCode, CommunicationCnst.MessageType messageType)
        {
            MessageCode = messageCode;
            Sender = CommunicationHelper.GetLocalIpAddress();
            MessageType = messageType;
        }

        public MessageBase(CommunicationCnst.Messages messageCode, CommunicationCnst.MessageType messageType)
            :this((int)messageCode, messageType)
        {
        }
    }
}
