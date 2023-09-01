using LIB.Communication.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Messages.Base
{
    [Serializable]
    public class ConnectionConfirmation : MessageBase
    {
        public int TestCode = (new Random()).Next(-2147483648, 2147483647);
        public ConnectionConfirmation()
            : base(CommunicationCnst.Messages.ConnectionConfirmation, CommunicationCnst.MessageType.ClientToHost)
        {
        }
    }
}
