using LIB.Communication.Constants;
using LIB.Communication.Messages.Base;
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
        public override byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter br = new BinaryWriter(ms))
            {
                br.Write(MessageCode);
                //br.Write((short)MessageType);
                br.Write(SenderId.ToByteArray());
                br.Write(UserId.ToByteArray());

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
                    this.MessageCode = br.ReadInt32();
                    //this.MessageType = (CommunicationCnst.MessageType)br.ReadInt16();
                    this.SenderId = new Guid(br.ReadBytes(16)); //GUID is 16 bytes
                    this.UserId = new Guid(br.ReadBytes(16));
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
