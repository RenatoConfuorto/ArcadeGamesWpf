using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIB.UserControls
{
    /// <summary>
    /// Logica di interazione per Timer.xaml
    /// </summary>
    public partial class Timer : UserControl , INotifyPropertyChanged
    {
        public Timer()
        {
            InitializeComponent();
        }
        #region RotationAngle
        private double _rotationAngle = 0;
        public double RotationAngle
        {
            get => _rotationAngle; 
            private set
            {
                _rotationAngle = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region ContentHeight
        public double ContentHeight
        {
            get => GetContentHeight();
        }
        private double GetContentHeight()
        {
            double contentHeight = 0;
            double StartingTime = (double)GetValue(StartingTimeProperty);
            double RemainingTime = this.Time;
            double MaxHeight = this.BarHeight - ((BorderThickness.Top + BorderThickness.Bottom) + (Padding.Top + Padding.Bottom));
            //MaxHeight : contentHeight = StartingTime : RemainingTime => 
            //contentHeight = (MaxHeight * RemainingTime) / StartingTime
            if(StartingTime > 0)contentHeight = (MaxHeight * RemainingTime) / StartingTime;
            return contentHeight;
        }
        #endregion

        #region Orientation
        private static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation),
                typeof(Orientation),
                typeof(Timer),
                new PropertyMetadata(Orientation.Vertical, OrientationPropertyChanged));
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        private static void OrientationPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender != null && sender is Timer timer)
            {
                if(timer.Orientation == Orientation.Vertical) 
                {
                    timer.RotationAngle = 0;
                }
                else if(timer.Orientation == Orientation.Horizontal) 
                {
                    timer.RotationAngle = 90;
                }
            }
        }
        #endregion
        #region BarHeight
        private static readonly DependencyProperty BarHeightProperty =
            DependencyProperty.Register(nameof(BarHeight),
                typeof(double),
                typeof(Timer));
        public double BarHeight
        {
            get => (double)GetValue(BarHeightProperty);
            set => SetValue(BarHeightProperty, value);
        }
        #endregion

        #region Time
        private static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time),
                typeof(double),
                typeof(Timer),
                new PropertyMetadata((double)0, TimePropertyChanged));
        public double Time
        {
            get => (double)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }
        private static void TimePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender != null && sender is Timer timer)
            {
                timer.NotifyPropertyChanged(nameof(ContentHeight));
            }
        }
        #endregion

        #region StartingTime
        private static DependencyProperty StartingTimeProperty =
            DependencyProperty.Register(nameof(StartingTime),
                typeof(double),
                typeof(Timer),
                new PropertyMetadata((double)0));
        public double StartingTime
        {
            get => (double)GetValue(StartingTimeProperty);
            set => SetValue(StartingTimeProperty, value);
        }
        #endregion

        #region TimerEnabled
        private static readonly DependencyProperty TimerEnabledProperty = 
            DependencyProperty.Register(nameof(TimerEnabled),
                typeof(bool),
                typeof(Timer),
                new PropertyMetadata(false, TimerEnabledChanged));
        public bool TimerEnabled
        {
            get => (bool)GetValue(TimerEnabledProperty);
            set => SetValue (TimerEnabledProperty, value);
        }
        private static void TimerEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region PlayerName
        private static readonly DependencyProperty PlayerNameProperty =
            DependencyProperty.Register(nameof(PlayerName),
                typeof(string),
                typeof(Timer));
        public string PlayerName
        {
            get => (string)GetValue(PlayerNameProperty);
            set => SetValue(PlayerNameProperty, value);
        }
        #endregion
        #region NotifyPrpertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
