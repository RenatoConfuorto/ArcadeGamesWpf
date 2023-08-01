using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIB.UserControls
{
    /// <summary>
    /// Logica di interazione per GameMessage.xaml
    /// </summary>
    public partial class GameMessage : UserControl
    {
        public GameMessage()
        {
            InitializeComponent();
        }

        #region GameOverText
        public static readonly DependencyProperty GameOverTextProperty =
            DependencyProperty.Register(nameof(GameOverText),
                typeof(string),
                typeof(GameMessage));
        public string GameOverText
        {
            get => (string)GetValue(GameOverTextProperty);
            set => SetValue(GameOverTextProperty, value);
        }
        #endregion

        #region VisibilityTrigger
        public static readonly DependencyProperty VisibilityTriggerProperty =
            DependencyProperty.Register(nameof(VisibilityTrigger),
                typeof(bool),
                typeof(GameMessage));
        public bool VisibilityTrigger
        {
            get => (bool)GetValue(VisibilityTriggerProperty);
            set => SetValue(VisibilityTriggerProperty, value);
        }
        #endregion
    }
}
