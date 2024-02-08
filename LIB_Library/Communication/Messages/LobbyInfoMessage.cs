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
        //public override byte[] Serialize()
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    using (BinaryWriter bw = new BinaryWriter(ms))
        //    {
        //        bw.Write(MessageCode);
        //        //bw.Write((short)MessageType);
        //        bw.Write(SenderId.ToByteArray());
        //        bw.Write(_hostIp);

        //        for(int i = 0; i < MULTIPLAYER_USERS_LIMIT; i++)
        //        {
        //            bw.Write(Users[i].Serialize());
        //        }

        //        return ms.ToArray();
        //    }
        //}

        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(_hostIp);
            for (int i = 0; i < MULTIPLAYER_USERS_LIMIT; i++)
            {
                bw.Write(Users[i].Serialize());
            }
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.HostIp = br.ReadString(HOST_IP_LENGTH);
            this.Users  = br.ReadObjectList<OnlineUser>(MULTIPLAYER_USERS_LIMIT).ToArray();
        }
        //public override void Deserialize(byte[] data)
        //{
        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream(data))
        //        using (BinaryReader br = new BinaryReader(ms))
        //        {
        //            this.MessageCode    = br.ReadInt32();
        //            //this.MessageType  = (CommunicationCnst.MessageType)br.ReadInt16();
        //            this.SenderId       = br.ReadGuid();
        //            this.HostIp         = br.ReadString(HOST_IP_LENGTH);
        //            this.Users          = br.ReadObjectList<OnlineUser>(MULTIPLAYER_USERS_LIMIT).ToArray();
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}
        #endregion
    }
}
