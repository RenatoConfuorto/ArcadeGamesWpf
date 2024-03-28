using LIB_Com.Constants;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Messages
{
    public class GameInitCommandMessage : MessageBase
    {
        public GameInitCommandMessage()
            :base(CommunicationCnst.Messages.GameInitCommandMessage, new Guid())
        {
        }
        #region Serialize/Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
        }
        #endregion
    }
}
