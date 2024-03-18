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
using LIB.Helpers;

namespace LIB_Com.Messages
{
    public class LobbyStatusAndSettings : MessageBase
    {
        public LobbyStatus lobbyStatus { get; set; }
        //public short ChatStatus
        //{
        //    get => Status != null ? Status.ChatStatus : (short)0;
        //}
        //public bool bChatStatus
        //{
        //    get => Convert.ToBoolean(ChatStatus);
        //}

        //public short GameId { get; set; }
        //public OnlineSettingsBase GameSettings { get; set; }

        public LobbyStatusAndSettings()
            :base(CommunicationCnst.Messages.LobbyStatusAndSettings, new Guid())
        {
            
        }
        public LobbyStatusAndSettings(LobbyStatus status)
            : base(CommunicationCnst.Messages.LobbyStatusAndSettings, new Guid())
        {
            this.lobbyStatus = status;
        }

        //public LobbyStatusAndSettings(bool status)
        //    :this(Convert.ToInt16(status))
        //{
        //}
        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            //bw.Write(ChatStatus);
            //bw.Write(GameId);
            //if(GameSettings != null) bw.WriteObject(GameSettings);
            bw.WriteObject(lobbyStatus);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            //this.ChatStatus = br.ReadInt16();
            //this.GameId     = br.ReadInt16();
            //this.GameSettings = CommunicationHelper.DeserializeOnlineSettings(br, this.GameId);
            this.lobbyStatus     = br.ReadDynamicObject<LobbyStatus>();
        }
        #endregion
    }
}
