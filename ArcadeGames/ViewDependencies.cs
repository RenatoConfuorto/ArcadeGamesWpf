﻿using ArcadeGames.ViewModels;
using ArcadeGames.Views;
using Core.Dependency;
using LIB.Constants;
using Core.Interfaces.Navigation;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.ViewModels;
using Core.Interfaces.ViewModels;

namespace ArcadeGames
{
    public class ViewDependencies : DependencyInjectionBase
    {
        public override void InjectDependencies()
        {
            AddDependency<IViewModelBase, MainWindowViewModel>(ViewNames.MainWindow);
            AddDependency<IViewModelBase, HomeViewModel>(ViewNames.Home);
        }
    }
}
