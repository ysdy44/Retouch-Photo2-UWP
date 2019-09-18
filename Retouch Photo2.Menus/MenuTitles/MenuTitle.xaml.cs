using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Title of IMenu.
    /// </summary>
    public sealed partial class MenuTitle : UserControl
    {
        //@Content
        /// <summary> TextBlock's Text. </summary>
        public string Text { set => this.TextBlock.Text = value; get => this.TextBlock.Text; }
        /// <summary> RootGrid. </summary>
        public UIElement RootGrid => this._RootGrid;
      
        /// <summary> StateButton. </summary>
        public UIElement StateButton => this._StateButton;
        /// <summary> CloseButton. </summary>
        public UIElement CloseButton => this._CloseButton;
    
        /// <summary> BackButton. </summary>
        public UIElement BackButton => this._BackButton;
        /// <summary> ResetButton. </summary>
        public UIElement ResetButton => this._ResetButton;


        /// <summary> Sets the state. </summary>
        public MenuState State
        {
            set
            {
                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.FlyoutShow:
                        {
                            this._RootGrid.Background = this.UnAccentColor;
                            this._CloseButton.Visibility = Visibility.Collapsed;
                        }
                        break;
                    case MenuState.OverlayExpanded:
                    case MenuState.OverlayNotExpanded:
                        {
                            this._RootGrid.Background = this.AccentColor;
                            this._CloseButton.Visibility = Visibility.Visible;
                        }
                        break;
                }

                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.OverlayNotExpanded:
                        {
                            this.StateIcon.Glyph = "\uE196";
                        }
                        break;
                    case MenuState.FlyoutShow:
                    case MenuState.OverlayExpanded:
                        {
                            this.StateIcon.Glyph = "\uE840";
                        }
                        break;
                }
            }
        }


        #region DependencyProperty


        /// <summary> Sets or gets the control is on the second page. </summary>
        public bool IsSecondPage
        {
            get { return (bool)GetValue(IsSecondPageProperty); }
            set { SetValue(IsSecondPageProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MenuTitle.IsSecondPage" /> dependency property. </summary>
        public static readonly DependencyProperty IsSecondPageProperty = DependencyProperty.Register(nameof(IsSecondPage), typeof(bool), typeof(MenuTitle), new PropertyMetadata(false, (sender, e) =>
        {
            MenuTitle con = (MenuTitle)sender;

            if (e.NewValue is bool value)
            {
                if (value)
                {
                    con.FirstGrid.Visibility = Visibility.Collapsed;
                    con.SecondGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    con.FirstGrid.Visibility = Visibility.Visible;
                    con.SecondGrid.Visibility = Visibility.Collapsed;
                }
            }
        }));


        /// <summary> Sets or gets ResetButton's Visibility. </summary>
        public Visibility ResetButtonVisibility
        {
            get { return (Visibility)GetValue(ResetButtonVisibilityProperty); }
            set { SetValue(ResetButtonVisibilityProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MenuTitle.ResetButtonVisibility" /> dependency property. </summary>
        public static readonly DependencyProperty ResetButtonVisibilityProperty = DependencyProperty.Register(nameof(ResetButtonVisibility), typeof(Visibility), typeof(MenuTitle), new PropertyMetadata(Visibility.Collapsed));
        

        #endregion


        //@Construct
        public MenuTitle()
        {
            this.InitializeComponent();
        }
    }
}
