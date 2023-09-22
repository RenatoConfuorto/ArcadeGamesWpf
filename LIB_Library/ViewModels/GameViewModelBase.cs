using Core.Attributes;
using Core.Commands;
using Core.Dependency;
using Core.Helpers;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using Core.Views;
using LIB.Attributes;
using LIB.Entities;
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
    public abstract class GameViewModelBase<C> : ContentViewModel
        where C : CellEntityBase, new()
    {
        #region Private Fields
        private bool _isGameEnabled = true;
        private bool _isGameOver = false;
        private string _gameOverMessage = String.Empty;
        private User _mainUser;
        private string SettingsPopupName;

        private ObservableCollection<C> _cells;
        #endregion

        #region Commands
        public RelayCommand SettingsCommand { get; set; }
        #endregion

        #region Public Properties
        public bool IsGameEnabled
        {
            get => _isGameEnabled;
            set => SetProperty(ref _isGameEnabled, value);
        }
        public bool IsGameOver
        {
            get => _isGameOver;
            set => SetProperty(ref _isGameOver, value);
        }
        public string GameOverMessage
        {
            get => _gameOverMessage;
            set => SetProperty(ref _gameOverMessage, value);
        }
        public ObservableCollection<C> Cells
        {
            get => _cells;
            set => SetProperty(ref _cells, value);
        }
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
        public GameViewModelBase(string viewName, string parentView = null) : base(viewName, parentView)
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
            base.OnInitialized();
            MenageGameUsers();
            InitGame();
        }
        public override void Dispose()
        {
            base.Dispose();
            //InitGame();
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
                OnSettingsReceied(popResult);
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
        protected virtual void OnSettingsReceied(object settings)
        {
        }
        protected abstract ObservableCollection<C> GenerateGrid();
        protected abstract void SaveGameResults();
        protected virtual void MenageGameUsers()
        {
            MainUser = UserManager.GetMainLoggedInUser();
        }
        protected virtual void InitGame()
        {
            Cells = GenerateGrid();
            IsGameEnabled = true;
            IsGameOver = false;
            MenageGameUsers();
        }
        protected virtual void EndGame()
        {
            IsGameEnabled = false;
            IsGameOver = true;
            SaveGameResults();
        }
        #endregion
    }
}
