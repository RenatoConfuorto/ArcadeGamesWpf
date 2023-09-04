using Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Tris.Common
{
    /// <summary>
    /// Logica di interazione per SuperTrisCell.xaml
    /// </summary>
    public partial class SuperTrisCell : UserControl , INotifyPropertyChanged
    {
        private double _panelWidth;
        private double _panelHeight;
        public RelayCommand SubCellClicked { get; set; }
        public double PanelWidth { get; set; }
        public double PanelHeight { get; set; }
        public SuperTrisCell()
        {
            InitializeComponent();
        }

        #region CellContent
        public static readonly DependencyProperty CellContentPorperty =
            DependencyProperty.Register(nameof(CellContent),
                                        typeof(SuperTrisEntity),
                                        typeof(SuperTrisCell),
                                        new PropertyMetadata(null, new PropertyChangedCallback(CellContentChanged)));

        public SuperTrisEntity CellContent
        {
            get => (SuperTrisEntity)GetValue(CellContentPorperty);
            set => SetValue(CellContentPorperty, value);
        }
        #endregion

        #region CellFontSize
        public static readonly DependencyProperty CellFontSizeProperty =
            DependencyProperty.Register(nameof(CellFontSize),
                typeof(double),
                typeof(SuperTrisCell));

        public double CellFontSize
        {
            get => (double)GetValue(CellFontSizeProperty);
            set => SetValue(CellFontSizeProperty, value);
        }
        #endregion

        #region SUB CELL PROPERTIES
        #region SubCellWidth
        public static readonly DependencyProperty SubCellWidthProperty =
            DependencyProperty.Register(nameof(SubCellWidth),
                typeof(double),
                typeof(SuperTrisCell),
                new PropertyMetadata((double)0, new PropertyChangedCallback(CellDimensionChanged)));
        public double SubCellWidth
        {
            get => (double)GetValue(SubCellWidthProperty);
            set => SetValue(SubCellWidthProperty, value);
        }
        #endregion 

        #region SubCellHeight
        public static readonly DependencyProperty SubCellHeightProperty =
            DependencyProperty.Register(nameof(SubCellHeight),
                typeof(double),
                typeof(SuperTrisCell),
                new PropertyMetadata((double)0, new PropertyChangedCallback(CellDimensionChanged)));
        public double SubCellHeight
        {
            get => (double)GetValue(SubCellHeightProperty);
            set => SetValue(SubCellHeightProperty, value);
        }
        #endregion 

        #region SubCellFontSize
        public static readonly DependencyProperty SubCellFontSizeProperty =
            DependencyProperty.Register(nameof(SubCellFontSize),
                typeof(double),
                typeof(SuperTrisCell));
        public double SubCellFontSize
        {
            get => (double)GetValue(SubCellFontSizeProperty);
            set => SetValue(SubCellFontSizeProperty, value);
        }
        #endregion

        #region SubCellMargin
        public static readonly DependencyProperty SubCellMarginProperty =
            DependencyProperty.Register(nameof(SubCellMargin),
                typeof(double),
                typeof(SuperTrisCell),
                new PropertyMetadata((double)0, new PropertyChangedCallback(CellDimensionChanged)));

        public event PropertyChangedEventHandler PropertyChanged;

        public double SubCellMargin
        {
            get => (double)GetValue(SubCellMarginProperty);
            set => SetValue(SubCellMarginProperty, value);
        }
        #endregion 
        #endregion
        private static void CellContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            if(sender is SuperTrisCell cell)
            {
                foreach(TrisEntity trisCell in cell.CellContent.SubCells)
                {
                    trisCell.cellClicked += cell.OnCellClicked;
                }
            }
        }

        private static void CellDimensionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is SuperTrisCell cell)
            {
                cell.PanelWidth = ((cell.SubCellWidth + 2 * cell.SubCellMargin) * 3);
                cell.PropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(nameof(PanelWidth)));

                cell.PanelHeight = ((cell.SubCellHeight + 2 * cell.SubCellMargin) * 3);
                cell.PropertyChanged?.Invoke(cell, new PropertyChangedEventArgs(nameof(PanelHeight)));
            }
        }

        private void OnCellClicked(int SubCellId)
        {
            if(CellContent != null) CellContent.MacroCellClicked?.Invoke(CellContent.CellId, SubCellId);
        }
    }
}
