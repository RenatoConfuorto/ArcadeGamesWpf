using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Communication.Events
{
    public class LobbyInfoRequestedEventArgs : EventArgs
    {
        public OnlineClient client { get ; set; }
        public LobbyInfoRequestedEventArgs(OnlineClient client)
        {
            this.client = client;
        }
    }
}
