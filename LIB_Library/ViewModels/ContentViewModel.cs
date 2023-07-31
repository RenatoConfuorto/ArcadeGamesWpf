using Core.Interfaces.Navigation;
using Core.Interfaces.ViewModels;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.ViewModels
{
    public class ContentViewModel : ViewModelBase
    {
        #region Private Fields
        private bool _isGameEnabled = true;
        #endregion

        

        #region Public Properties
        public bool IsGameEnabled
        {
            get => _isGameEnabled;
            set => SetProperty(ref _isGameEnabled, value);
        }
        #endregion

        #region Constructor
        public ContentViewModel(string parentView = null) : base(parentView)
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
        protected virtual void InitGame() 
        {
            IsGameEnabled = true;
        }
        #endregion
    }
}
