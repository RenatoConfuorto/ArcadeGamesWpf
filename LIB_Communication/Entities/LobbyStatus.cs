using Core.Entities;
using Core.Interfaces.Entities;
using LIB.Helpers;
using LIB_Com.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Entities
{
    public class LobbyStatus : SerializableBase
    {
        public short ChatStatus { get; set; }
        public bool bChatStatus
        {
            get => Convert.ToBoolean(ChatStatus);
        }

        public short GameId { get; set; }
        public OnlineSettingsBase GameSettings { get; set; }

        public LobbyStatus()
        {
        }

        public LobbyStatus(short chatStatus, short gameId, OnlineSettingsBase gameSettings)
        {
            this.ChatStatus = chatStatus;
            this.GameId = gameId;
            this.GameSettings = gameSettings;
        }
        #region Serialize/Deserialize
        public override void Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                this.ChatStatus = br.ReadInt16();
                this.GameId = br.ReadInt16();
                this.GameSettings = CommunicationHelper.DeserializeOnlineSettings(br, GameId);
            }
        }

        public override byte[] Serialize()
        {
            using(MemoryStream ms = new MemoryStream())
            using(BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(ChatStatus);
                bw.Write(GameId);
                if (GameSettings != null) bw.WriteObject(GameSettings);

                return ms.ToArray();
            }
        } 
        #endregion
    }
}
