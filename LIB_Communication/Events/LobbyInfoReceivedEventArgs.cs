using LIB_Com.Messages;
using LIB_Com.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Events
{
    public class LobbyInfoReceivedEventArgs : EventArgs
    {
        public string HostIp { get; set; }
        public IEnumerable<OnlineUser> Users { get; set; }

        public LobbyInfoReceivedEventArgs(string hostIp, IEnumerable<OnlineUser> users)
        {
            this.HostIp = hostIp;
            this.Users = users;
        }

        public LobbyInfoReceivedEventArgs(LobbyInfoMessage infoMessage) 
        { 
            this.HostIp = infoMessage.HostIp;
            this.Users = infoMessage.Users;
        }
    }
}
