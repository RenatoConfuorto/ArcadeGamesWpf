using Core.Attributes;
using Core.Commands;
using Core.Dependency;
using Core.Helpers;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using Core.Views;
using LIB.Attributes;
using LIB.Entities;
using LIB.Sounds;
using LIB.UserMng;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace LIB.ViewModels
{
    public abstract class LocalGameViewModelBase<C> : GameViewModelBase<C>
        where C : CellEntityBase, new()
    {
        #region Private Fields
        private User _mainUser;
        private string SettingsPopupName;
        #endregion

        #region Commands
        public RelayCommand SettingsCommand { get; set; }
        #endregion

        #region Public Properties
        public User MainUser
        {
            get => _mainUser;
            set
            {
                SetProperty(ref _mainUser, value);
                NotifyPropertyChanged(nameof(MainUserName));
            }
        }
        public string MainUserName
        {
            get => MainUser != null ? MainUser.Name : "Giocatore 1";
        }
        #endregion

        #region Constructor
        public LocalGameViewModelBase(string viewName, string parentView = null, object param = null) 
            : base(viewName, parentView, param)
        {
            GetSettingsPopupName();
        }
        #endregion

        #region Override Methods
        protected override void SetCommandExecutionStatus()
        {
            base.SetCommandExecutionStatus();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            SettingsCommand = new RelayCommand(SettingsCommandExecute, SettingsCommandCanExecute);
        }
        protected override void OnInitialized()
        {
            MenageGameUsers();
            base.OnInitialized();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Private Methods
        private void GetSettingsPopupName()
        {
            SettingsPopup attribute = this.GetType().GetCustomAttribute<SettingsPopup>();
            if(attribute != null)
            {
                SettingsPopupName = attribute.PopupName;
                SettingsCommand.RaiseCanExecuteChanged();
            }
        }
        private void SettingsCommandExecute(object param)
        {
            GameSettingsBase settings = PrepareDataForPopup();
            PopUp popUp = new PopUp(SettingsPopupName, settings);
            object popResult = popUp.Show();
            OnPopupClosed();
            if (popResult != null)
            {
                OnSettingsReceived(popResult);
                InitGame();
            }
        }
        private bool SettingsCommandCanExecute(object param)
        {
            return !String.IsNullOrEmpty(SettingsPopupName);
        }
        #endregion

        #region Protected Methods
        protected virtual GameSettingsBase PrepareDataForPopup()
        {
            return new GameSettingsBase();
        }
        protected virtual void OnPopupClosed()
        {

        }
        protected virtual void OnSettingsReceived(object settings)
        {
        }
        protected abstract void SaveGameResults();
        protected virtual void MenageGameUsers()
        {
            MainUser = UserManager.GetMainLoggedInUser();
        }
        #endregion
    }
}
