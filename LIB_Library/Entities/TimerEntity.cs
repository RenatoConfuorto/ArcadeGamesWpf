using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities
{
    public class TimerEntity : NotifyerPropertyChangedBase
    {
        private string _playerName;
        private int _originalTime;
        private int _time;
        private bool _timerEnabled;

        public string PlayerName
        {
            get => _playerName;
            set => SetProperty(ref _playerName, value);
        }
        public int OriginalTime
        {
            get => _originalTime;
            set => SetProperty(ref _originalTime, value);
        }
        public int Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }
        public bool TimerEnabled
        {
            get => _timerEnabled;
            set => SetProperty(ref _timerEnabled, value);
        }
    }
}
