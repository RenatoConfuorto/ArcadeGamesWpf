using Core.Commands;
using LIB.Constants;
using LIB.Settings;
using LIB.Sounds;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
    /// Logica di interazione per MainVolumeControl.xaml
    /// </summary>
    public partial class MainVolumeControl : UserControl, INotifyPropertyChanged
    {
        private object volumeDisabledResource = Application.Current.Resources["VolumeDisabledIcon"]; 
        private object volumeResource = Application.Current.Resources["VolumeIcon"];
        private System.Timers.Timer _popupCloseTimer;
        public MainVolumeControl()
        {
            MainButtonClickCommand = new RelayCommand(MainButtonClickCommandExecute);
            SwitchEnabledVolumeStatusCommand = new RelayCommand(SwitchEnabledVolumeStatusCommandExecute);
            InitializeComponent();
        }

        #region MainButtonClickCommand
        public RelayCommand MainButtonClickCommand { get; set; }
        private void MainButtonClickCommandExecute(object param)
        {
            IsOpen = !IsOpen;
        }
        #endregion
        #region SwitchEnabledVolumeStatusCommand
        public RelayCommand SwitchEnabledVolumeStatusCommand { get; set; }
        private void SwitchEnabledVolumeStatusCommandExecute(object param)
        {
            if (Volume <= 0) Volume = DefaultSettings.DEFAULT_VOLUME_LEVEL;
            else Volume = 0;
        } 
        #endregion

        #region IsOpen
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(MainVolumeControl), new PropertyMetadata(false));
        #endregion

        #region IsVolumeDisabled

        public bool IsVolumeDisabled
        {
            get => Volume <= 0;
        }
        #endregion
        #region Volume
        private int _volume = (int)(SoundsManagment.Volume * 100d);
        public int Volume 
        {
            get => _volume;
            set
            {
                SetProperty(ref _volume, value);
                NotifyPropertyChanged(nameof(IsVolumeDisabled));
                SoundsManagment.SetGlobalVolume(value);
            }
        }
        #endregion

        #region NotifyPropertyChangedInterface
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void SetProperty<T>(ref T _field, T value, [CallerMemberName] string propertyName = null)
        {
            _field = value;
            NotifyPropertyChanged(propertyName);
        }
        #endregion
    }
}
