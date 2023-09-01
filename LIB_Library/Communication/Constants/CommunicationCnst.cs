using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Constants
{
    public class CommunicationCnst
    {
        public const int DEFAULT_PORT = 8800;
        //public const int CONNECTION_TIMEOUT = 30000; //30s
        public const int CONNECTION_MAX_TRIES = 30;
        public const int CONNECTION_RETRY_WAIT = 3000; //3s
        public const int CONNECTION_CHECK_INTERVAL = 500; //0.5s
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
            ConnectionConfirmation = 1000,
            ConnectionConfirmed = 1001,
            LobbyLogin = 1002,
            LobbyInfo = 1003,
            LobbyChatMessage = 1004
        }

        struct Test
        {
            int valore1;
            long valore2;
            char valore3;
        }
    }
}
