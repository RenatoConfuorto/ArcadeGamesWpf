using MemoryGame.Common.Entities;
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

namespace MemoryGame.Common.UserControls
{
    /// <summary>
    /// Logica di interazione per CardCell.xaml
    /// </summary>
    public partial class CardCell : UserControl
    {
        public CardCell()
        {
            InitializeComponent();
        }

        #region CellContent
        private static readonly DependencyProperty CellContentProperty =
            DependencyProperty.Register(nameof(CellContent),
                typeof(CardEntity),
                typeof(CardCell));
        public CardEntity CellContent
        {
            get => (CardEntity)GetValue(CellContentProperty);
            set => SetValue(CellContentProperty, value);
        }
        #endregion

        #region CellWidth
        private static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register(nameof(CellWidth),
                typeof(double),
                typeof(CardCell), 
                new PropertyMetadata((double)0));
        public double CellWidth
        {
            get => (double)GetValue(CellWidthProperty);
            set => SetValue(CellWidthProperty, value);
        }
        #endregion

        #region CellHeight
        private static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register(nameof(CellHeight),
                typeof(double),
                typeof(CardCell),
                new PropertyMetadata((double)0));
        public double CellHeight
        {
            get => (double)GetValue(CellHeightProperty);
            set => SetValue(CellHeightProperty, value);
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CellContent.cellClicked?.Invoke(CellContent.CellId);
        }
    }
}
