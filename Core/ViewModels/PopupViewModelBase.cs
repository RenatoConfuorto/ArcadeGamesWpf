using Core.Commands;
using Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class PopupViewModelBase : ViewModelBase
    {
        #region Private Fields
        private bool _isOperationConfirmed;
        public object _returnValue;
        #endregion

        #region Event
        public event ClosePopupEvent closePopup;
        #endregion

        #region Commands
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        #endregion

        #region Public Properties
        public bool IsOperationConfirmed
        {
            get => _isOperationConfirmed;
            set => SetProperty(ref _isOperationConfirmed, value);
        }
        #endregion

        #region Constructor
        public PopupViewModelBase(string viewName, object param = null) : base(viewName, null, param)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override void InitCommands()
        {
            base.InitCommands();
            OkCommand = new RelayCommand(OkCommandExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Private Methods
        private void OkCommandExecute(object param)
        {
            IsOperationConfirmed = true;
            closePopup?.Invoke(GetPopReturnData());
        }
        private void CancelCommandExecute(object param)
        {
            IsOperationConfirmed = false;
            closePopup?.Invoke(null);
        }
        #endregion

        #region Protected Methods
        protected virtual object GetPopReturnData()
        {
            return null;
        }
        #endregion
    }
}
