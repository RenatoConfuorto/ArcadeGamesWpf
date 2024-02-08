using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Helpers.CommunicationHelper;
using static LIB.Communication.Constants.CommunicationCnst;

namespace LIB.Communication.Messages
{
    [Serializable]
    public class SendUserNameToHost : MessageBase
    {
        private char[] _userName = new char[USER_NAME_LENGTH];
        public string UserName 
        {
            get => new string(_userName); 
            set => SetString(ref _userName, value, USER_NAME_LENGTH);
        }
        public SendUserNameToHost() { }
        public SendUserNameToHost(string userName)
            :base(CommunicationCnst.Messages.SendUserNameToHost, new Guid()/*, CommunicationCnst.MessageType.ClientToHost*/)
        {
            this.UserName = userName;
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
                br.Write(_userName);

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
                    this.SenderId       = new Guid(br.ReadBytes(GUID_LENGTH));
                    this._userName      = br.ReadChars(USER_NAME_LENGTH);
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

    }
}
