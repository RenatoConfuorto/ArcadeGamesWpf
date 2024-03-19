using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Constants
{
    public class CommunicationCnst
    {
        public const int DEFAULT_PORT                   = 8800;
        public const int DEFAULT_BUFFER_SIZE            = 1024;
        //public const int CONNECTION_TIMEOUT             = 30000; //30s
        public const int CONNECTION_MAX_TRIES           = 1200;
        public const int CONNECTION_RETRY_WAIT          = 100; //0.1s
        public const int CONNECTION_CHECK_INTERVAL      = 500; //0.5s

        public const int MULTIPLAYER_USERS_LIMIT        = 5;
        #region Fields Length
        public const int GUID_LENGTH                    = 16;
        public const int USER_NAME_LENGTH               = 14;
        public const int HOST_IP_LENGTH                 = 20;
        //public const int ONLINE_USER_LENGTH             = USER_NAME_LENGTH + GUID_LENGTH;
        public const int LOBBY_CHAT_MESSAGE_SIZE        = 80;
        public const int ONLINE_SETTINGS_POPUP_NAME_LEN = 80;
        #endregion
        public enum Mode
        {
            Host = 0,
            Client = 1,
        }

        public enum MessageType
        {
            MultiDirection = 0,
            HostToCliet = 1,
            ClientToHost = 2,
        }

        public enum Messages
        {
            SendDataConfirmation        = 1000,
            SendUserNameToHost          = 1005,
            LobbyInfoMessage            = 1010,
            SendUpdatedUserList         = 1015,
            LobbyChatMessage            = 1020,
            LobbyStatusAndSettings      = 1025,
            HostDisconnectedMessage     = 1026,
            ClientDisconnectedMessage   = 1027
        }

        #region Static References
        public static readonly int USER_NAME_LEN             = USER_NAME_LENGTH;
        public static readonly int LOBBY_CHAT_MESSAGE_SZ     = LOBBY_CHAT_MESSAGE_SIZE;
        #endregion

        #region View Parameters Names
        public const string USER_MODE   = "Mode";
        public const string USER_BROKER = "Broker";
        public const string HOST_IP     = "HostIp";
        public const string USERS_LIST  = "Users";
        #endregion
    }
}
