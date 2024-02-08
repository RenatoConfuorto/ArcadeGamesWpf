using LIB_Com.Constants;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Helpers.CommunicationHelper;
using static LIB_Com.Constants.CommunicationCnst;
using LIB_Com.Extensions;

namespace LIB_Com.Messages
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
        //public override byte[] Serialize()
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    using (BinaryWriter bw = new BinaryWriter(ms))
        //    {
        //        bw.Write(MessageCode);
        //        //bw.Write((short)MessageType);
        //        bw.Write(SenderId.ToByteArray());
        //        bw.Write(_userName);

        //        return ms.ToArray();
        //    }
        //}

        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(_userName);
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
        //            this.UserName       = br.ReadString(USER_NAME_LENGTH);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.UserName   = br.ReadString(USER_NAME_LENGTH);
        }
        #endregion

    }
}
