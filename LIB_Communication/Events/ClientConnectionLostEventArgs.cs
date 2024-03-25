using LIB_Com.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Events
{
    public class ClientConnectionLostEventArgs : EventArgs
    {
        public OnlineUser User { get; set; }

        public ClientConnectionLostEventArgs()
        {
        }
        public ClientConnectionLostEventArgs(OnlineUser user)
            :base()
        {
            this.User = user;
        }
    }
}
