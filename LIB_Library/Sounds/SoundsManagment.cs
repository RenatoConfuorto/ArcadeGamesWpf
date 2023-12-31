﻿using Core.Helpers;
using LIB.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LIB.Sounds
{
    public class SoundsManagment
    {
        public const string MainBackground = ".\\Sounds\\8-bit_baseBackground.wav";

        public const string TrisSoundPenClickingTwice = ".\\Sounds\\Tris\\mixkit-pen-clicking-twice-2371.wav";
        public const string TrisSoundPenWrite = ".\\Sounds\\Tris\\mixkit-pen-marker-line-2998.wav";
        public const string TrisSoundPenWriteLong = ".\\Sounds\\Tris\\mixkit-short-pencil-writing-2376.wav";
        public const string TrisSoundTickingClock = ".\\Sounds\\Tris\\wall-clock-ticks-quartz-clock-25480.wav";

        public const string MemorySoundCard_1 = ".\\Sounds\\Memory\\card-sounds-35956.wav";
        public const string MemorySoundCard_2 = ".\\Sounds\\Memory\\cardsound32562-37691.wav";
        public const string MemorySoundCard_3 = ".\\Sounds\\Memory\\flipcard-91468.wav";
        public const string MemorySoundPointScored = ".\\Sounds\\Memory\\point_scored.wav";
        public const string MemorySoundCardsShuffle = ".\\Sounds\\Memory\\shuffle-cards-46455.wav";

        #region Public Methods
        public static void ChangeBackground(string background) => NewBackground = background;

        public static void PlaySoundSingle(string soundPath)
        {
            try
            {
                    MediaPlayer player = new MediaPlayer();
                    player.Open(new Uri(Path.GetFullPath(soundPath)));
                    player.Volume = Volume;
                    player.Play();
                //Task.Run(() =>
                //{
                //});
            }
            catch (Exception ex)
            {
                MessageDialogHelper.ShowInfoMessage(ex.Message);
            }
        }
        #endregion

        #region Background Music Managment
        private static string _currentBackground;
        private static TimeSpan _currentPosition;
        private static string _newBackground;
        private static MediaPlayer _player;
        private static double _volume = -1;

        private static string NewBackground
        {
            get => _newBackground;
            set
            {
                _newBackground = value;
                if (String.IsNullOrEmpty(value) && _player != null)
                {
                    _currentBackground = value;
                    _player.Stop();
                    _player = null;
                }
                else if (_newBackground != _currentBackground)
                {
                    _currentBackground = value;
                    StartBackground();
                }
            }
        }

        public static double Volume
        {
            get
            {
                if(_volume < 0)
                {
                    SetVolume(GlobalSettings.GlobalVolume);
                }
                return _volume;
            }
            private set
            {
                _volume = value;
                if(_player != null)
                {
                    _player.Volume = value;
                }
            }
        }

        private static void StartBackground()
        {
            _player = new MediaPlayer();
            //_player.Load();
            _player.Open(new Uri(Path.GetFullPath(_currentBackground)));
            _player.Volume = Volume;
            _player.MediaEnded += ResetBackgroundForLoop;
            _player.Play();
        }
        private static void ResetBackgroundForLoop(object sender, EventArgs e)
        {
            if (_player == null) return;
            _player.Position = TimeSpan.Zero;
            _player.Play();
        }
        public static void SetGlobalVolume(int NewVolume)
        {
            int _volume = SetVolume(NewVolume);
            GlobalSettings.GlobalVolume = _volume;
        }
        private static int SetVolume(int NewVolume)
        {
            int _volume;
            if (NewVolume < 0) _volume = 0;
            else if (NewVolume > 100) _volume = 1;
            else _volume = NewVolume;

            Volume = _volume / 100d;
            return _volume;
        }
        public static void PauseBackground()
        {
            _currentPosition = _player.Position;
            _player.Stop();
        }
        public static void ResumeBackground()
        {
            if (_currentPosition != null)
            {
                _player.Position = _currentPosition;
                _player.Play();
            }
        }
        #endregion
    }
}
