using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Retouch_Photo2 Menu 's Button.
    /// </summary>
    public sealed partial class MenuButton : UserControl, IExpanderButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        

        //@Delegate
        public Action<ExpanderState> StateChanged { get; set; }
        
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        public ToolTip ToolTip => this._ToolTip;

        public FrameworkElement Self => this;


        //@VisualState
        ClickMode _vsClickMode;
        ExpanderState _vsMenuState = ExpanderState.Hide;
        public VisualState VisualState
        {
            get
            {
                if (this._vsMenuState == ExpanderState.FlyoutShow) return this.FlyoutShow;

                if (this._vsMenuState == ExpanderState.Hide)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }

                if (this._vsMenuState == ExpanderState.Overlay || this._vsMenuState == ExpanderState.OverlayNotExpanded)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Overlay;
                        case ClickMode.Hover: return this.PointerOverOverlay;
                        case ClickMode.Press: return this.PressedOverlay;
                    }
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }

        public ExpanderState State
        {
            set
            {
                this._vsMenuState = value;
                this.VisualState = this.VisualState;//State         
            }
        }


        //@Construct
        public MenuButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State 

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;

            this.Tapped += (s, e) =>
            {
                ExpanderState state = this.GetState(this._vsMenuState);
                this.StateChanged?.Invoke(state);
            };
        }

        private ExpanderState GetState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Hide: return ExpanderState.FlyoutShow;
                case ExpanderState.FlyoutShow: return ExpanderState.Hide;

                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.FlyoutShow;
        }
    }
}