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

namespace Tris.Common
{
    /// <summary>
    /// Logica di interazione per TrisCell.xaml
    /// </summary>
    public partial class TrisCell : UserControl
    {
        public TrisCell()
        {
            InitializeComponent();
            Btn.Click += Button_Click;
        }

        #region CellContent
        public static readonly DependencyProperty CellContentPorperty =
            DependencyProperty.Register(nameof(CellContent),
                                        typeof(TrisEntity),
                                        typeof(TrisCell),
                                        new PropertyMetadata(null, new PropertyChangedCallback(CellContentChanged)));

        public TrisEntity CellContent
        {
            get => (TrisEntity)GetValue(CellContentPorperty);
            set => SetValue(CellContentPorperty, value);
        }
        #endregion

        #region CellFontSize
        public static readonly DependencyProperty CellFontSizeProperty =
            DependencyProperty.Register(nameof(CellFontSize),
                typeof(double),
                typeof(TrisCell));

        public double CellFontSize
        {
            get => (double)GetValue(CellFontSizeProperty);
            set => SetValue(CellFontSizeProperty, value);
        } 
        #endregion
        private static void CellContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(CellContent != null)
            {
                CellContent.cellClicked?.Invoke(CellContent.CellId);
            }
        }
    }
}
