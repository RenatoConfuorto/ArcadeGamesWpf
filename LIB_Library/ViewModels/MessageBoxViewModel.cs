using Core.Commands;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LIB.ViewModels
{
    public enum MessageType
    {
        Info = 0,
        Confirmation = 1,
        Error = 2,
        Status = 3
    }
    public class MessageBoxViewModel : NotifyerPropertyChangedBase
    {
        public bool MessageResult = false;
        public EventHandler CloseBox;

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private Visibility _confirmationOptions = Visibility.Collapsed;
        public Visibility ConfirmationOptions
        {
            get => _confirmationOptions;
            set => SetProperty(ref _confirmationOptions, value);
        }

        private Visibility _closeOption = Visibility.Visible;
        public Visibility CloseOption
        {
            get => _closeOption;
            set => SetProperty(ref _closeOption, value);
        }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public MessageBoxViewModel(MessageType type, string message)
        {
            SetOptionVisibility(type);
            Message = message;
            OkCommand = new RelayCommand(OkCommandExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        private void SetOptionVisibility(MessageType type)
        {
            switch (type)
            {
                case MessageType.Confirmation:
                    CloseOption = Visibility.Collapsed;
                    ConfirmationOptions = Visibility.Visible;
                    break;
                case MessageType.Info:
                case MessageType.Error:
                    CloseOption = Visibility.Visible;
                    ConfirmationOptions = Visibility.Collapsed;
                    break;
                case MessageType.Status:
                    CloseOption = Visibility.Collapsed;
                    ConfirmationOptions = Visibility.Collapsed;
                    break;
            }
        }

        private void OkCommandExecute(object param)
        {
            MessageResult = true;
            CloseBox?.Invoke(this, EventArgs.Empty);
        }

        private void CancelCommandExecute(object param)
        {
            CloseBox?.Invoke(this, EventArgs.Empty);
        }
    }
}
