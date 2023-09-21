using Core.Commands;
using Core.ViewModels;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ViewModels
{
    public class SettingsViewModelBase<T> : PopupViewModelBase
        where T : GameSettingBase
    {
        #region Private fields
        private T _settings;
        private bool _settingsConfirmed;
        #endregion

        #region Public Properties
        public T Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }
        public bool SettingsConfirmed
        {
            get => _settingsConfirmed;
            set => SetProperty(ref _settingsConfirmed, value);
        }
        #endregion

        #region Constructor
        public SettingsViewModelBase(string viewName, object param)
            : base(viewName, param)
        {
        }
        #endregion

        #region Ovverride Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            
        }
        protected override object GetPopReturnData() => Settings;
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
