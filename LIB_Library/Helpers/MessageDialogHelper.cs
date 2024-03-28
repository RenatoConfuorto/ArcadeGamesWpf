using LIB.Views;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Helpers
{
    public class MessageDialogHelper
    {
        public static void ShowInfoMessage(string message)
        {
            LIB.Views.MessageBox messagebox = new LIB.Views.MessageBox(MessageType.Info, message);
            messagebox.ShowDialog();
        }

        public static bool ShowConfirmationRequestMessage(string message)
        {
            LIB.Views.MessageBox messageBox = new LIB.Views.MessageBox(MessageType.Confirmation, message);
            messageBox.ShowDialog();
            return messageBox.result;
        }

        public static LIB.Views.MessageBox ShowStatusMessage(string message)
        {
            LIB.Views.MessageBox messageBox = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox = new LIB.Views.MessageBox(MessageType.Status, message);
                messageBox.Show();
            });
            return messageBox;
        }

        public static void CloseMessageBox(LIB.Views.MessageBox messageBox)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageBox.Close();
            });
        }
    }
}
