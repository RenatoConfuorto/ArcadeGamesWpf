using LIB_Com.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Events
{
    public class NewOnlineUserEventArgs : EventArgs
    {
        public OnlineUser NewUser { get; set; }

        public NewOnlineUserEventArgs(OnlineUser newUser)
        {
            this.NewUser = newUser;
        }
    }
}
