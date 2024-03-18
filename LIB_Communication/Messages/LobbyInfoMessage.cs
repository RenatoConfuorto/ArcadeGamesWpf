using LIB_Com.Constants;
using LIB_Com.Messages.Base;
using LIB_Com.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Helpers.CommunicationHelper;
using static LIB_Com.Constants.CommunicationCnst;
using LIB_Com.Extensions;
using LIB.Helpers;

namespace LIB_Com.Messages
{
    public class LobbyInfoMessage : MessageBase
    {
        private char[] _hostIp = new char[HOST_IP_LENGTH];
        private OnlineUser[] _users = new OnlineUser[MULTIPLAYER_USERS_LIMIT];

        public string HostIp
        {
            get => new string(_hostIp);
            set => SetString(ref _hostIp, value, HOST_IP_LENGTH);
        }
        public OnlineUser[] Users
        {
            get => _users;
            set => SetArray(ref _users, value, MULTIPLAYER_USERS_LIMIT);
        }
        
        public LobbyStatus lobbyStatus { get; set; }
        //public short ChatStatus { get; set; }
        //public bool bChatStatus
        //{
        //    get => Convert.ToBoolean(ChatStatus);
        //}

        //public short GameId { get; set; }
        //public OnlineSettingsBase GameSettings { get; set; }

        public LobbyInfoMessage() { }
        public LobbyInfoMessage(string hostIp, IEnumerable<OnlineUser> users, LobbyStatus status)
            : base(CommunicationCnst.Messages.LobbyInfoMessage, new Guid())
        {
            this.HostIp = hostIp;
            this.Users = users.ToArray();
            this.lobbyStatus = status;
        }
        public LobbyInfoMessage(string hostIp, IEnumerable<OnlineUser> users, short chatStatus, short gameId, OnlineSettingsBase gameSettings)
            :this(hostIp, users, new LobbyStatus(chatStatus, gameId, gameSettings))
        {
        }
        public LobbyInfoMessage(string hostIp, IEnumerable<OnlineUser> users, bool chatStatus, short gameId, OnlineSettingsBase gameSettings)
            :this(hostIp, users, Convert.ToInt16(chatStatus), gameId, gameSettings)
        {
        }

        #region Serialize / Deserialize

        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(_hostIp);
            bw.WriteObjectList(Users);
            //bw.Write(ChatStatus);
            //bw.Write(GameId);
            //if(GameSettings != null) bw.WriteObject(GameSettings); 
            bw.WriteObject(lobbyStatus);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.HostIp     = br.ReadString(HOST_IP_LENGTH);
            this.Users      = br.ReadObjectList<OnlineUser>(MULTIPLAYER_USERS_LIMIT).ToArray();
            //this.ChatStatus = br.ReadInt16();
            //this.GameId     = br.ReadInt16();
            //this.GameSettings = CommunicationHelper.DeserializeOnlineSettings(br, GameId);
            this.lobbyStatus     = br.ReadDynamicObject<LobbyStatus>();
        }
        #endregion
    }
}
