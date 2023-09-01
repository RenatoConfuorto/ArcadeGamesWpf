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
    public class LobbyChatMessage : MessageBase
    {
        public string MessageText;
        public LobbyChatMessage() 
            : base(CommunicationCnst.Messages.LobbyChatMessage, CommunicationCnst.MessageType.MultiDirection)
        {
        }
    }
}
