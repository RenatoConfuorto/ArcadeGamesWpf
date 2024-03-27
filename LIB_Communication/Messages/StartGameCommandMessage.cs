using LIB_Com.Constants;
using LIB_Com.Messages.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB_Com.Constants.CommunicationCnst;
using static LIB.Helpers.CommunicationHelper;
using LIB_Com.Extensions;

namespace LIB_Com.Messages
{
    public class StartGameCommandMessage : MessageBase
    {
        public int GameId { get; set; }

        private char[] _gameViewName = new char[ONLINE_GAME_VIEW_NAME_LEN];
        public string GameViewName
        {
            get => new string(_gameViewName);
            set => SetString(ref _gameViewName, value, ONLINE_GAME_VIEW_NAME_LEN);
        }


        public StartGameCommandMessage(int gameId, string gameViewName)
            :this()
        {
            this.GameId = gameId;
            this.GameViewName = gameViewName;
        }
        public StartGameCommandMessage()
            :base(CommunicationCnst.Messages.StartGameCommandMessage, new Guid())
        {
        }
        #region Serialize / Deserialize
        public override void SerializeData(BinaryWriter bw)
        {
            base.SerializeData(bw);
            bw.Write(GameId);
            bw.Write(_gameViewName);
        }

        public override void DeserializeData(BinaryReader br)
        {
            base.DeserializeData(br);
            this.GameId         = br.ReadInt32();
            this.GameViewName   = br.ReadString(ONLINE_GAME_VIEW_NAME_LEN); 
        }
        #endregion
    }
}
