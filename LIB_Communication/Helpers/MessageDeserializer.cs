using LIB.Helpers;
using LIB_Com.Constants;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Helpers
{
    public class MessageDeserializer
    {
        public static MessageBase DeserializeMessage(byte[] data)
        {
            //get the messageCode
            int messageCode = BitConverter.ToInt32(data, 0);
            switch ((CommunicationCnst.Messages)messageCode)
            {
                case CommunicationCnst.Messages.SendDataConfirmation:
                    return CommunicationHelper.DeserializeObject<SendDataConfirmation>(data);
                case CommunicationCnst.Messages.SendUserNameToHost:
                    return CommunicationHelper.DeserializeObject<SendUserNameToHost>(data);
                case CommunicationCnst.Messages.LobbyInfoMessage:
                    return CommunicationHelper.DeserializeObject<LobbyInfoMessage>(data);
                case CommunicationCnst.Messages.SendUpdatedUserList:
                    return CommunicationHelper.DeserializeObject<SendUpdatedUserList>(data);
                case CommunicationCnst.Messages.LobbyChatMessage:
                    return CommunicationHelper.DeserializeObject<LobbyChatMessage>(data);
                case CommunicationCnst.Messages.LobbyStatusAndSettings:
                    return CommunicationHelper.DeserializeObject<LobbyStatusAndSettings>(data);
                case CommunicationCnst.Messages.HostDisconnectedMessage:
                    return CommunicationHelper.DeserializeObject<HostDisconnectedMessage>(data);
                case CommunicationCnst.Messages.ClientDisconnectedMessage:
                    return CommunicationHelper.DeserializeObject<ClientDisconnectedMessage>(data);
                default: return null;
            }
        }
    }
}
