using Core.Entities;
using Core.Interfaces.DbBrowser;
using LIB.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LIB.Entities
{
    public class User : NotifyerPropertyChangedBase
    {
        [XmlIgnore]
        public UserProxy Proxy;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _created;
        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }

        private DateTime _updated;
        public DateTime Updated
        {
            get => _updated;
            set => SetProperty(ref _updated, value);
        }

        private DateTime _lastConnected;
        public DateTime LastConnected
        {
            get => _lastConnected;
            set => SetProperty(ref _lastConnected, value);
        }

        private TimeSpan _totalActiveTime = new TimeSpan();
        [XmlIgnore]
        public TimeSpan TotalActiveTime
        {
            get => _totalActiveTime;
            set => SetProperty(ref _totalActiveTime, value);
        }
        [XmlElement(nameof(TotalActiveTime))]
        public long TotalActiveTimeTicks
        {
            get => TotalActiveTime.Ticks;
            set => TotalActiveTime = new TimeSpan(value);
        }
        private bool _isDefaultAccess;
        public bool IsDefaultAccess
        {
            get => _isDefaultAccess;
            set => SetProperty(ref _isDefaultAccess, value);
        }

        private int _autoLoginOrder;
        public int AutoLoginOrder
        {
            get => _autoLoginOrder;
            set => SetProperty(ref _autoLoginOrder, value);
        }
    }
}
