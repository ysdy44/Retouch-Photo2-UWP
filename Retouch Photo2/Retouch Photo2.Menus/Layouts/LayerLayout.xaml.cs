﻿using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class LayerLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public bool IsOpen
        {
            set
            {
                if (this._Layout.State== MenuState.RootExpanded)
                {
                    this.LayerControl.IsOpen = value;
                }
            }
        }
        public MenuState State { set => this._Layout.State = value; }
        public UIElement Self => this;

        public Flyout Flyout => this._Flyout;

        public UIElement StateButton => this._Layout.StateButton;
        public UIElement CloseButton => this._Layout.CloseButton;
        public UIElement TitlePanel => this._Layout.TitlePanel;


        //@Construct
        public LayerLayout()
        {
            this.InitializeComponent();
        }
    }
}