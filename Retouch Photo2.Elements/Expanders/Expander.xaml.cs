using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public partial class Expander : UserControl, IExpander
    {
        //@Content
        public string Title { set => this.TitleTextBlock.Text = value; get => this.TitleTextBlock.Text; }
        public UIElement MainPage { set => this.MainPageBorder.Child = value; get => this.MainPageBorder.Child; }
        public UIElement SecondPage { set => this.SecondPageBorder.Child = value; get => this.SecondPageBorder.Child; }
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                if (this._vsIsSecondPage != value)
                {
                    this.HeightRectangle.VerticalAlignment = VerticalAlignment.Top;
                    if (value) this.HeightStoryboardMainToSecond.Begin();//Storyboard
                    else this.HeightStoryboardSecondToMain.Begin();//Storyboard
                }

                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public Visibility ResetButtonVisibility { set => this.ResetButton.Visibility = value; get => this.ResetButton.Visibility; }
        public Action Reset { get; set; }


        //@VisualState
        bool _vsIsSecondPage = false;
        ExpanderState _vsState = ExpanderState.Hide;
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
        public double PostionX { get => Canvas.GetLeft(this.Layout); set => Canvas.SetLeft(this.Layout, value); }
        public double PostionY { get => Canvas.GetTop(this.Layout); set => Canvas.SetTop(this.Layout, value); }


        //@Construct     
        public Expander()
        {
            this.InitializeComponent();
            this.ConstructWidthStoryboard();
            this.ConstructHeightStoryboard();
            this.Tapped += (s, e) => e.Handled = true;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize()
        {
            this.VisualState = this.VisualState;//State 

            /////////////////////////////////

            this.Button.Self.Tapped += (s, e) =>
            {
                if (this.State== ExpanderState.Hide) this.CalculatePostion(this.Button.Self, this.PlacementMode);
                        
                this.State = this.GetButtonState(this.State);
            };

            this.CloseButton.Tapped += (s, e) => this.State = ExpanderState.Hide;
            this.StateButton.Tapped += (s, e) => this.State = this.GetState(this.State);

            this.BackButton.Tapped += (s, e) => this.IsSecondPage = false;
            if (this.Reset != null) this.ResetButton.Tapped += (s, e) => this.Reset();

            /////////////////////////////////

            this.TitleGrid.DoubleTapped += (s, e) => this.State = this.GetState(this.State);

            this.TitleGrid.ManipulationMode = ManipulationModes.All;
            this.TitleGrid.ManipulationStarted += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postionX = this.PostionX;
                this._postionY = this.PostionY;

                this.Move?.Invoke(); //Delegate
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