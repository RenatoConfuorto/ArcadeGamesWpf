using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using LIB.Entities;
using LIB.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Communication.Constants.CommunicationCnst;
using static LIB.Helpers.CommunicationHelper;

namespace LIB.Communication.Messages
{
    public class SendUpdatedUserList : MessageBase
    {
        private OnlineUser[] _users = new OnlineUser[MULTIPLAYER_USERS_LIMIT];

        public OnlineUser[] Users
        {
            get => _users;
            set => SetArray(ref _users, value, MULTIPLAYER_USERS_LIMIT);
        }

        public SendUpdatedUserList() { }

        public SendUpdatedUserList(IEnumerable<OnlineUser> users)
            :base(CommunicationCnst.Messages.SendUpdatedUserList, new Guid()/*, MessageType.HostToCliet*/)
        {
            Users = users.ToArray();
        }

        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            //for (int i = 0; i < MULTIPLAYER_USERS_LIMIT; i++)
            //{
            //    bw.Write(Users[i].Serialize());
            //}
            bw.WriteObjectList(Users);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.Users  = br.ReadObjectList<OnlineUser>(MULTIPLAYER_USERS_LIMIT).ToArray();
        }
        #endregion
    }
}
