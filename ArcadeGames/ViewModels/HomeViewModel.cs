﻿using LIB.Base;
using LIB.Constants;
using LIB.Interfaces.Navigation;
using LIB.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames.ViewModels
{
    public class HomeViewModel : ContentViewModel
    {
        #region Private Fields
        #endregion

        #region Command
        public RelayCommand TrisPageCommand { get; set; }
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public HomeViewModel() :this(null) { }
        public HomeViewModel(INavigationService navService) : base(navService)
        {
        }
        #endregion

        #region Override Methods
        protected override void InitCommands()
        {
            base.InitCommands();
            TrisPageCommand = new RelayCommand(TrisPageCommandExecute);
        }
        #endregion

        #region Private Methods
        private void TrisPageCommandExecute(object param)
        {
            ChangeView(ViewNames.TrisHomePageViewModel);
        }
        #endregion
    }
}
