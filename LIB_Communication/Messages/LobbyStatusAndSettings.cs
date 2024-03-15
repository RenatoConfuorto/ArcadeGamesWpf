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
        /// <summary>
        /// 0 => Disabled, 1 => Enabled
        /// </summary>
        public short ChatStatus { get; set; }
        public bool bChatStatus
        {
            get => Convert.ToBoolean(ChatStatus);
        }

        public short GameId { get; set; }
        public OnlineSettingsBase GameSettings { get; set; }

        public LobbyStatusAndSettings()
            :base(CommunicationCnst.Messages.LobbyStatusAndSettings, new Guid())
        {
            
        }
        public LobbyStatusAndSettings(short status)
            : base(CommunicationCnst.Messages.LobbyStatusAndSettings, new Guid())
        {
            this.ChatStatus = status;
        }

        public LobbyStatusAndSettings(bool status)
            :this(Convert.ToInt16(status))
        {
        }
        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(ChatStatus);
            bw.Write(GameId);
            if(GameSettings != null) bw.WriteObject(GameSettings);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.ChatStatus = br.ReadInt16();
            this.GameId     = br.ReadInt16();
            this.GameSettings = CommunicationHelper.DeserializeOnlineSettings(br, this.GameId);
        }
        #endregion
    }
}
