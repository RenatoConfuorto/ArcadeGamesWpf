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
        public override byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(MessageCode);
                //br.Write((short)MessageType);
                br.Write(SenderId.ToByteArray());

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

                    List<OnlineUser> users = new List<OnlineUser>();
                    for(int i = 0; i < MULTIPLAYER_USERS_LIMIT; i++)
                    {
                        OnlineUser user = new OnlineUser();
                        user.Deserialize(br.ReadBytes(user.GetSize()));
                        users.Add(user);
                    }
                    Users = users.ToArray();
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
