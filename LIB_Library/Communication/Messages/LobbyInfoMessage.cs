using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Helpers.CommunicationHelper;
using static LIB.Communication.Constants.CommunicationCnst;
using LIB.Extensions;

namespace LIB.Communication.Messages
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

        public LobbyInfoMessage() { }
        public LobbyInfoMessage(string hostIp, IEnumerable<OnlineUser> users)
            :base(CommunicationCnst.Messages.LobbyInfoMessage, new Guid())
        {
            this.HostIp = hostIp;
            this.Users = users.ToArray(); 
        }

        #region Serialize / Deserialize
        public override byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(MessageCode);
                //br.Write((short)MessageType);
                br.Write(SenderId.ToByteArray());
                br.Write(_hostIp);

                for(int i = 0; i < MULTIPLAYER_USERS_LIMIT; i++)
                {
                    br.Write(Users[i].Serialize());
                }

                return ms.ToArray();
            }
        }

        public override void Deserialize(byte[] data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.MessageCode    = br.ReadInt32();
                    //this.MessageType  = (CommunicationCnst.MessageType)br.ReadInt16();
                    this.SenderId       = br.ReadGuid();
                    this._hostIp        = br.ReadChars(HOST_IP_LENGTH);
                    //users
                    //List<OnlineUser> users = new List<OnlineUser>();
                    //for(int i =0; i < MULTIPLAYER_USERS_LIMIT; i++)
                    //{
                    //    OnlineUser user = new OnlineUser();
                    //    user.Deserialize(br.ReadBytes(user.GetSize()));
                    //    users.Add(user);
                    //}
                    //SetArray(ref _users, users, MULTIPLAYER_USERS_LIMIT);
                    this.Users          = br.ReadObjectList<OnlineUser>(MULTIPLAYER_USERS_LIMIT).ToArray();
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
