using Core.Helpers;
using Core.Interfaces.Logging;
using LIB.Helpers;
using LIB_Com.Constants;
using LIB_Com.Messages;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Helpers
{
    public class MessageDeserializer
    {
        static ILogger logger = LoggerHelper.GetSystemLogger();
        public static MessageBase DeserializeMessage(byte[] data)
        {
            //get the messageCode
            int messageCode = BitConverter.ToInt32(data, 0);
            MessageBase message = null;
            //switch ((CommunicationCnst.Messages)messageCode)
            //{
            //    case CommunicationCnst.Messages.Watchdog:
            //        return CommunicationHelper.DeserializeObject<Watchdog>(data);
            //    case CommunicationCnst.Messages.SendDataConfirmation:
            //        return CommunicationHelper.DeserializeObject<SendDataConfirmation>(data);
            //    case CommunicationCnst.Messages.SendUserNameToHost:
            //        return CommunicationHelper.DeserializeObject<SendUserNameToHost>(data);
            //    case CommunicationCnst.Messages.LobbyInfoMessage:
            //        return CommunicationHelper.DeserializeObject<LobbyInfoMessage>(data);
            //    case CommunicationCnst.Messages.SendUpdatedUserList:
            //        return CommunicationHelper.DeserializeObject<SendUpdatedUserList>(data);
            //    case CommunicationCnst.Messages.LobbyChatMessage:
            //        return CommunicationHelper.DeserializeObject<LobbyChatMessage>(data);
            //    case CommunicationCnst.Messages.LobbyStatusAndSettings:
            //        return CommunicationHelper.DeserializeObject<LobbyStatusAndSettings>(data);
            //    case CommunicationCnst.Messages.HostDisconnectedMessage:
            //        return CommunicationHelper.DeserializeObject<HostDisconnectedMessage>(data);
            //    case CommunicationCnst.Messages.ClientDisconnectedMessage:
            //        return CommunicationHelper.DeserializeObject<ClientDisconnectedMessage>(data);
            //    case CommunicationCnst.Messages.ClientConnectionLost:
            //        return CommunicationHelper.DeserializeObject<ClientConnectionLost>(data);
            //    default: return null;
            //}
            CommunicationCnst.Messages messageDef;
            if(Enum.TryParse(messageCode.ToString(), out messageDef))
            {
                string messageName = messageDef.ToString();
                Type mesType = Type.GetType($"LIB_Com.Messages.{messageName}");
                if(mesType == null)
                {
                    logger.LogError("Impossibile trovare il messaggio <{0}> nel namespace LIB_Com.Messages", messageName);
                    return message;
                }
                message = Activator.CreateInstance(mesType) as MessageBase;
                message.Deserialize(data);
            }
            else
            {
                logger.LogError("Message Code sconosciuto: <{0}>", messageCode);
            }
            return message;
        }
    }
}
