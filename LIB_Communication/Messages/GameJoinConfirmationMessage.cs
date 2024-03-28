using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;
using static LIB.Helpers.CommunicationHelper;
using LIB_Com.Constants;
using LIB_Com.Extensions;

namespace LIB_Com.Messages
{
    public class GameJoinConfirmationMessage : MessageBase
    {
        private char[] _gameViewName = new char[ONLINE_GAME_VIEW_NAME_LEN];
        public string GameViewName
        {
            get => new string(_gameViewName);
            set => SetString(ref _gameViewName, value, ONLINE_GAME_VIEW_NAME_LEN);
        }

        public GameJoinConfirmationMessage(string gameViewName)
            :this()
        {
            this.GameViewName = gameViewName;
        }

        public GameJoinConfirmationMessage()
            :base(CommunicationCnst.Messages.GameJoinConfirmationMessage, new Guid())
        {
        }

        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(_gameViewName);
        }
        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.GameViewName = br.ReadString(ONLINE_GAME_VIEW_NAME_LEN);
        }
        #endregion
    }
}
