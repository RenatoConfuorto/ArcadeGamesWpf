using LIB_Com.Constants;
using LIB_Com.Entities;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;
using static LIB.Helpers.CommunicationHelper;
using LIB_Com.Extensions;

namespace LIB_Com.Messages
{
    public class LobbyChatMessage : MessageBase
    {
        private char[] _textMessage = new char[LOBBY_CHAT_MESSAGE_SIZE];
        public string TextMessage
        {
            get => new string(_textMessage);
            set => SetString(ref _textMessage, value, LOBBY_CHAT_MESSAGE_SIZE);
        }
        public OnlineUser User { get; set; }
        
        public LobbyChatMessage() { }
        public LobbyChatMessage(OnlineUser user, string message)
            : base(CommunicationCnst.Messages.LobbyChatMessage, new Guid())
        {
            this.User = user;
            this.TextMessage = message;
        }

        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.WriteObject(User);
            bw.Write(_textMessage);
        }


        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.User           = br.ReadObject<OnlineUser>();
            this.TextMessage    = br.ReadString(LOBBY_CHAT_MESSAGE_SIZE);
        }
        #endregion
    }
}
