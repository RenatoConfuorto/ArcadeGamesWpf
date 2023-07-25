using LIB.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Helpers
{
    public class MessageDialogHelper
    {
        public static void ShowInfoMessage(string message)
        {
            MessageBox messagebox = new MessageBox(ViewModels.MessageType.Info, message);
            messagebox.ShowDialog();
        }

        public static bool ShowConfirmationRequestMessage(string message)
        {
            MessageBox messageBox = new MessageBox(ViewModels.MessageType.Confirmation, message);
            messageBox.ShowDialog();
            return messageBox.result;
        }
    }
}
