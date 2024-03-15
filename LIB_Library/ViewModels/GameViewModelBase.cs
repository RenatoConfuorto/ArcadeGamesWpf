using Core.Commands;
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

namespace LIB.ViewModels
{
    public abstract class GameViewModelBase<C> : ContentViewModel
        where C : CellEntityBase
    {
        #region Private Fields
        private bool _isGameEnabled = true;
        private bool _isGameOver = false;
        private string _gameOverMessage = String.Empty;

        private ObservableCollection<C> _cells;
        #endregion

        #region Commands
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
        #endregion

        #region Constructor
        public GameViewModelBase(string viewName, string parentView = null, object param = null) 
            : base(viewName, parentView, param)
        {
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
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitGame();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        protected void PlaySound(string sound)
        {
            SoundsManagment.PlaySoundSingle(sound);
        }
        protected abstract ObservableCollection<C> GenerateGrid();
        protected virtual void InitGame()
        {
            Cells = GenerateGrid();
            IsGameEnabled = true;
            IsGameOver = false;
        }
        protected virtual void EndGame()
        {
            IsGameEnabled = false;
            IsGameOver = true;
        }
        #endregion
    }
}
