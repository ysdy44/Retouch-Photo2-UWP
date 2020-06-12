using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Button of <see cref="IMenu"/>.
    /// </summary>
    public sealed partial class MenuButton : UserControl, IExpanderButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content       
        /// <summary> ContentPresenter's Child. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        /// <summary> ToolTip </summary>  
        public ToolTip ToolTip => this._ToolTip;
        /// <summary> Self </summary>  
        public FrameworkElement Self => this;


        //@VisualState
        ClickMode _vsClickMode;
        ExpanderState _vsMenuState = ExpanderState.Hide;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
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
        /// <summary> VisualState's ClickMode. </summary>
        public ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }
        /// <summary> VisualState's ExpanderState. </summary>
        public ExpanderState ExpanderState
        {
            set
            {
                this._vsMenuState = value;
                this.VisualState = this.VisualState;//State         
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a MenuButton. 
        /// </summary>
        public MenuButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State 

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}