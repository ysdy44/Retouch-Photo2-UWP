using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Expander of <see cref="IMenu"/>.
    /// </summary>
    public partial class MenuExpander : UserControl
    {
        
        //@Content
        /// <summary> TextBlock's Text. </summary>
        public string Title { set => this.TitleTextBlock.Text = value; get => this.TitleTextBlock.Text; }
        /// <summary> TitleGrid. </summary>
        public FrameworkElement TitleGrid => this._TitleGrid;

        /// <summary> StateButton. </summary>
        public FrameworkElement StateButton => this._StateButton;
        /// <summary> CloseButton. </summary>
        public FrameworkElement CloseButton => this._CloseButton;

        /// <summary> BackButton. </summary>
        public FrameworkElement BackButton => this._BackButton;
        /// <summary> ResetButton. </summary>
        public FrameworkElement ResetButton => this._ResetButton;

        /// <summary> MainPageBorder's Child. </summary>
        public UIElement MainPage { set => this.MainPageBorder.Child = value; get => this.MainPageBorder.Child; }
        /// <summary> SecondPageBorder's Child. </summary>
        public UIElement SecondPage { set => this.SecondPageBorder.Child = value; get => this.SecondPageBorder.Child; }


        //@VisualState
        bool _vsIsSecondPage = false;
        MenuState _vsState = MenuState.Hide;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case MenuState.Hide:
                        return this.Hide;

                    case MenuState.FlyoutShow:
                        {
                            if (this._vsIsSecondPage) return this.FlyoutShowSecondPage;
                            return this.FlyoutShow ;
                        }

                    case MenuState.OverlayNotExpanded:
                        return this.OverlayNotExpanded;

                    case MenuState.Overlay: 
                        {
                            if (this._vsIsSecondPage) return this.OverlaySecondPage;
                            return this.Overlay;
                        }

                    default: return this.Normal;
                }
            }
            set
            {

                VisualStateManager.GoToState(this, value.Name, false);

                if (value==this.Hide)
                {
                    this.HeightFrame.Value = 0;
                    this.HeightStoryboard.Begin();//Storyboard
                }
            }
        }
        
        public MenuState State
        {
            get => this._vsState;
            set
            {
                if (value== MenuState.OverlayNotExpanded)
                {
                    this.HeightFrame.Value = 0;
                    this.HeightStoryboard.Begin();//Storyboard
                }

                this._vsState = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }


        //@Construct
        public MenuExpander()
        {
            this.InitializeComponent();
            this.ConstructHeightStoryboard();
            this.Tapped += (s, e) => e.Handled = true;
        }

        private double HeightBegin
        {
            set
            {
                this.HeightFrame.Value = value;
                this.HeightStoryboard.Begin();//Storyboard
            }
        }

        private void ConstructHeightStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.HeightKeyFrames, this.HeightStoryboardRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFrames, "(UIElement.Height)");
            
            // MainPage is Visible, SecondPage is Collapsed.
            this.TitleToggleButton.Unchecked += (s, e) => this.HeightBegin = this.MainPageBorder.ActualHeight;
            // MainPage is Collapsed, SecondPage is Visible.
            this.TitleToggleButton.Checked += (s, e) => this.HeightBegin = this.SecondPageBorder.ActualHeight;
            // MainPage and SecondPage are Collapsed.
            this.TitleToggleButton.Indeterminate += (s, e) => this.HeightBegin = 0;

            this.MainPageBorder.SizeChanged += (s, e) => this.HeightBegin = e.NewSize.Height;
            this.SecondPageBorder.SizeChanged += (s, e) => this.HeightBegin = e.NewSize.Height;
        }

    }
}