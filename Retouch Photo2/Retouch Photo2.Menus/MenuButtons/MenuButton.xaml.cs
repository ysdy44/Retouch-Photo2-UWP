using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class MenuButton : UserControl
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content 
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        /// <summary> MenuButton's MenuState. </summary>
        public MenuState State
        {
            set
            {
                this._vsMenuState = value;
                this.VisualState = this.VisualState;//State         
            }
        }
        /// <summary> ToolTip. </summary>
        public ToolTip ToolTip => this._ToolTip;

        //@VisualState
        MenuState _vsMenuState;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsMenuState == MenuState.FlyoutShow) return this.Flyout;

                if (this._vsMenuState == MenuState.Hide)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }

                if (this._vsMenuState == MenuState.Overlay || this._vsMenuState == MenuState.OverlayNotExpanded)
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
        

        //@Construct
        public MenuButton()
        {
            this.InitializeComponent();
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
        public MenuButton(object centerContent) : this()
        {
            this.ContentPresenter.Content = centerContent;
        }

    }
}