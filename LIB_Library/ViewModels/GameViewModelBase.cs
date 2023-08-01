using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ViewModels
{
    public abstract class GameViewModelBase<C> : ContentViewModel
        where C : CellEntityBase, new()
    {
        #region Private Fields
        private bool _isGameEnabled = true;
        private bool _isGameOver = false;
        private string _gameOverMessage = String.Empty;

        private ObservableCollection<C> _cells;
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
        public GameViewModelBase(string viewName, string parentView = null) : base(viewName, parentView)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitGame();
        }
        public override void Dispose()
        {
            base.Dispose();
            InitGame();
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        protected abstract ObservableCollection<C> GenerateGrid();
        protected virtual void InitGame()
        {
            Cells = GenerateGrid();
            IsGameEnabled = true;
            IsGameOver = false;
        }
        protected void EndGame()
        {
            IsGameEnabled = false;
            IsGameOver = true;
        }
        #endregion
    }
}
