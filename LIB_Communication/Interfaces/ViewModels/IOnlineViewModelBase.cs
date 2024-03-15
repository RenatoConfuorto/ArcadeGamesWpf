using LIB.Entities;
using LIB_Com.Constants;
using LIB_Com.Entities;
using LIB_Com.Events;
using LIB_Com.MessageBrokers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Interfaces.ViewModels
{
    public interface IOnlineViewModelBase
    {
        bool IsUserHost { get; }
        bool IsUserClient { get; }
        string HostIp { get; }
        BindingList<OnlineUser> Users { get; }

        CommunicationCnst.Mode _userMode { get; }
        BrokerHost _brokerHost { get; }
        BrokerClient _brokerClient { get; }

        void OnMessageReceivedEvent(object sender, MessageReceivedEventArgs e);
    }
}
