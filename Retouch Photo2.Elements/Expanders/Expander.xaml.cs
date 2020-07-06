using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public abstract partial class Expander : UserControl
    {
        
        //@Content
        public string Title { get => this.TitleTextBlock.Text; protected set => this.TitleTextBlock.Text = value; }
        public FrameworkElement Self => this;

        public UIElement MainPage { get => this.MainPageBorder.Child; set => this.MainPageBorder.Child = value; }
        public UIElement SecondPage { get => this.SecondPageBorder.Child; set => this.SecondPageBorder.Child = value; }
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                if (this._vsIsSecondPage != value)
                {
                    if (value) this.TitleShowStoryboard.Begin();//Storyboard
                    else this.TitleFadeStoryboard.Begin();//Storyboard

                    this.HeightRectangle.VerticalAlignment = VerticalAlignment.Top;
                    if (value) this.HeightStoryboardMainToSecond.Begin();//Storyboard
                    else this.HeightStoryboardSecondToMain.Begin();//Storyboard
                }

                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public Visibility ResetButtonVisibility { get => this.ResetButton.Visibility; set => this.ResetButton.Visibility = value; }
        public abstract void Reset();


        //@VisualState
        bool _vsIsSecondPage = false;
        ExpanderState _vsState = ExpanderState.Hide;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case ExpanderState.Hide:
                        return this.Hide;

                    case ExpanderState.FlyoutShow:
                        return this._vsIsSecondPage ? this.FlyoutShowSecondPage : this.FlyoutShow;

                    case ExpanderState.OverlayNotExpanded:
                        return this.OverlayNotExpanded;

                    case ExpanderState.Overlay:
                        return this._vsIsSecondPage ? this.OverlaySecondPage : this.Overlay;

                    default:
                        return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        double _postionX;
        double _postionY;
        private double PostionX { get => Canvas.GetLeft(this); set => Canvas.SetLeft(this, value); }
        private double PostionY { get => Canvas.GetTop(this); set => Canvas.SetTop(this, value); }


        //@Construct     
        /// <summary>
        /// Initializes a Expander. 
        /// </summary>
        public Expander()
        {
            this.InitializeComponent();
            this.InitializeExpander();
            this.ConstructWidthStoryboard();
            this.ConstructHeightStoryboard();
            this.Tapped += (s, e) => e.Handled = true;
        }


        private void InitializeExpander()
        {
            this.VisualState = this.VisualState;//State 

            /////////////////////////////////

            this.Button.Self.Tapped += (s, e) =>
            {
                if (this.State== ExpanderState.Hide) this.CalculatePostion(this.Button.Self, this.PlacementMode);
                        
                this.State = this.GetButtonState(this.State);
            };

            this.CloseButton.Click += (s, e) => this.State = ExpanderState.Hide;
            this.StateButton.Click += (s, e) => this.State = this.GetState(this.State);

            this.BackButton.Click += (s, e) =>
            {
                this.Title = this.Button.Title;
                this.IsSecondPage = false;
            };
            this.ResetButton.Click += (s, e) => this.Reset();

            /////////////////////////////////

            this.TitleGrid.DoubleTapped += (s, e) => this.State = this.GetState(this.State);

            this.TitleGrid.ManipulationMode = ManipulationModes.All;
            this.TitleGrid.ManipulationStarted += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postionX = this.PostionX;
                this._postionY = this.PostionY;

                this.Move(); //Delegate
            };
            this.TitleGrid.ManipulationDelta += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postionX += e.Delta.Translation.X;
                this._postionY += e.Delta.Translation.Y;

                this.PostionX = this.GetBoundPostionX(this._postionX);
                this.PostionY = this.GetBoundPostionY(this._postionY);
            };
            this.TitleGrid.ManipulationCompleted += (s, e) =>
            {
                this._postionX = this.PostionX;
                this._postionY = this.PostionY;
            };
        }
        
    }
}