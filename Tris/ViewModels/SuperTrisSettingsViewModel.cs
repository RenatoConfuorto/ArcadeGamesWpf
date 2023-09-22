using Core.Attributes;
using Core.Commands;
using Core.ViewModels;
using LIB.Constants;
using LIB.Entities;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common.Entities;
using Tris.Views;

namespace Tris.ViewModels
{
    [ViewRef(typeof(SuperTrisMpSettings))]
    public class SuperTrisSettingsViewModel : SettingsViewModelBase<SuperTrisSettings>
    {
        #region Private fields
        private int _minutes;
        #endregion

        #region Public Properties
        public int Minutes
        {
            get => _minutes;
            set
            {
                SetProperty(ref _minutes, value);
                Settings.PlayersTime = value * 60;
                SetCommandExecutionStatus();
            }
        }
        #endregion

        #region Commands
        public RelayCommand AddMinutes { get; set; }
        public RelayCommand RemoveMinutes { get; set; }
        #endregion

        #region Constructor
        public SuperTrisSettingsViewModel(object param) 
            : base(ViewNames.SuperTrisMpSettings, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void GetViewParameter()
        {
            base.GetViewParameter();
            Minutes = Settings.PlayersTime / 60;
            SetCommandExecutionStatus();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            AddMinutes = new RelayCommand(AddMinutesExecute, AddMinutesCanExecute);
            RemoveMinutes = new RelayCommand(RemoveMinutesExecute, RemoveMinutesCanExecute);
        }
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
            AddMinutes.RaiseCanExecuteChanged();
            RemoveMinutes.RaiseCanExecuteChanged();
        }
        #endregion

        #region Private Methods
        private void AddMinutesExecute(object param)
        {
            Minutes++;
        }
        private bool AddMinutesCanExecute(object param) => true;
        private void RemoveMinutesExecute(object param)
        {
            Minutes--;
        }
        private bool RemoveMinutesCanExecute(object param) => Minutes > 1;
        #endregion

    }
}
