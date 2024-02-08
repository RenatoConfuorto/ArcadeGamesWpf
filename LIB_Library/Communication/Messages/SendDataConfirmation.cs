using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
using LIB.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Messages
{
    public class SendDataConfirmation : MessageBase
    {
        public Guid UserId { get; set; }
        public SendDataConfirmation() { }
        public SendDataConfirmation(Guid userId)
            : base(CommunicationCnst.Messages.SendDataConfirmation, new Guid()/*, CommunicationCnst.MessageType.HostToCliet*/)
        {
            this.UserId = userId;
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
        //        bw.Write(UserId.ToByteArray());

        //        return ms.ToArray();
        //    }
        //}

        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(UserId.ToByteArray());
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
                    this.UserId         = br.ReadGuid();
                }
            }
            catch (Exception e)
            {

            }
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.UserId = br.ReadGuid();
        }
        #endregion
    }
}
