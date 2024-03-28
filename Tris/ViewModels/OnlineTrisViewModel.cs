using Core.Attributes;
using Core.Helpers;
using LIB.Constants;
using LIB.Sounds;
using LIB_Com.Attributes;
using LIB_Com.Entities.OnlineGameSettings;
using LIB_Com.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tris.Common.Entities;
using Tris.Views;

namespace Tris.ViewModels
{
    [NonReloadblePage]
    [ViewRef(typeof(OnlineTrisView))]
    public class OnlineTrisViewModel : OnlineGameViewModelBase<TrisEntity, OnlineTrisSettings>
    {
        #region Private Fields
        #endregion

        #region Commands
        #endregion

        #region Public Properties
        #endregion

        #region Constructor
        public OnlineTrisViewModel(object param = null)
            : base(ViewNames.OnlineTris, ViewNames.MultiPlayerLobby, param)
        {
        }

        #endregion

        #region Override Methods
        protected override ObservableCollection<TrisEntity> GenerateGrid()
        {
            ObservableCollection<TrisEntity> result = new ObservableCollection<TrisEntity>();
            TrisEntity cell;
            for (int i = 0; i < 9; i++)
            {
                cell = new TrisEntity()
                {
                    CellId = i,
                    Text = null,
                };
                cell.cellClicked += OnCellClicked;
                result.Add(cell);
            }
            return result;
        }
        #endregion

        #region Private Methods
        private void OnCellClicked(int cellId)
        {

        }
        #endregion

        #region Protected Methods
        #endregion
    }
}
