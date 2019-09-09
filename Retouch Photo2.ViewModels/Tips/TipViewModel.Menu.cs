﻿using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels.Tips
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        private void SetMenusOpen(bool isOpen)
        {
            this.DebugMenu.IsOpen = isOpen;
            this.SelectionMenu.IsOpen = isOpen;
            this.OperateMenu.IsOpen = isOpen;
            this.AdjustmentMenu.IsOpen = isOpen;
            this.EffectMenu.IsOpen = isOpen;
            this.TransformerMenu.IsOpen = isOpen;
            this.ColorMenu.IsOpen = isOpen;
            this.ToolMenu.IsOpen = isOpen;
            this.LayerMenu.IsOpen = isOpen;
        }

        /// <summary> Debug. </summary>
        public IMenu DebugMenu;

        /// <summary> Selection. </summary>
        public IMenu SelectionMenu;
        /// <summary> Operate. </summary>
        public IMenu OperateMenu;

        /// <summary> Adjustment. </summary>
        public IMenu AdjustmentMenu;
        /// <summary> Effect. </summary>
        public IMenu EffectMenu;
        /// <summary> Transformer. </summary>
        public IMenu TransformerMenu;

        /// <summary> Color. </summary>
        public IMenu ColorMenu;

        /// <summary> Tool. </summary>
        public IMenu ToolMenu;
        /// <summary> Layer. </summary>
        public IMenu LayerMenu;

    }
}