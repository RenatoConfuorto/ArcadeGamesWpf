using System;
using System.Collections.Generic;
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

namespace ArcadeGames.Views
{
    /// <summary>
    /// Logica di interazione per MultiPlayerLobby.xaml
    /// </summary>
    public partial class MultiPlayerLobbyView : UserControl
    {
        public static double TEXT_MESSAGES_TEXTBLOCK_WIDTH = 0;
        public static double USER_NAME_TEXTBLOCK_WIDTH = 175;
        public MultiPlayerLobbyView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            //LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetTextMessagesColumnWidth();
        }

        private void SetTextMessagesColumnWidth()
        {
            double mainContainerWidth = MainContainer.ActualWidth;
            double mainContainerMargin = MainContainer.Margin.Left + MainContainer.Margin.Right;
            double scrollBarWidth = SystemParameters.ScrollWidth + 30; //Scrollbar width

            TEXT_MESSAGES_TEXTBLOCK_WIDTH = 
                mainContainerWidth 
                - mainContainerMargin 
                - scrollBarWidth 
                - USER_NAME_TEXTBLOCK_WIDTH - 
                25; // Label ": "
            //this.UpdateLayout();
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock tb && TEXT_MESSAGES_TEXTBLOCK_WIDTH != 0)
            {
                tb.Width    = TEXT_MESSAGES_TEXTBLOCK_WIDTH;
                tb.MaxWidth = TEXT_MESSAGES_TEXTBLOCK_WIDTH;
            }
        }
    }
}
