using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public partial class Expander : UserControl, IExpander
    {
        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public ExpanderState State
        {
            get => this._vsState;
            set
            {
                switch (value)
                {
                    case ExpanderState.Hide:
                        {
                            this.HeightFrame.Value = 0;
                            this.HeightStoryboard.Begin();//Storyboard
                        }
                        break;

                    case ExpanderState.FlyoutShow:
                        {
                            FlyoutPlacementMode placement = this.PlacementMode;
                            Point flyoutPostion = VisualUIElementHelper.GetFlyoutPostion(this.Button.Self, this.Layout, placement);
                            Point boundPostion = VisualUIElementHelper.GetBoundPostion(flyoutPostion, this.Layout);
                            VisualUIElementHelper.SetOverlayPostion(this.Layout, boundPostion);
                            this.Move?.Invoke(); //Delegate

                            if (this._vsState == ExpanderState.Hide)
                                this.Opened?.Invoke(); //Delegate 
                        }
                        break;

                    case ExpanderState.OverlayNotExpanded:
                        {
                            this.HeightFrame.Value = 0;
                            this.HeightStoryboard.Begin();//Storyboard
                        }
                        break;

                    default:
                        {
                            if (this._vsState == ExpanderState.FlyoutShow)
                                this.Closed?.Invoke(); //Delegate
                        }
                        break;
                }

                this.Button.State = value;
                this._vsState = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public FrameworkElement Layout { get; set; }
        public IExpanderButton Button { get; set; }

        public Action Reset { get; set; }


        public string Title { set => this.TitleTextBlock.Text = value; get => this.TitleTextBlock.Text; }
        public UIElement MainPage { set => this.MainPageBorder.Child = value; get => this.MainPageBorder.Child; }
        public UIElement SecondPage { set => this.SecondPageBorder.Child = value; get => this.SecondPageBorder.Child; }
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                if (value == false)
                {
                    this.ResetButton.Visibility = Visibility.Collapsed;
                }

                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public Visibility ResetButtonVisibility { set => this.ResetButton.Visibility = value; get => this.ResetButton.Visibility; }


        //@VisualState
        bool _vsIsSecondPage = false;
        ExpanderState _vsState = ExpanderState.Hide;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsState)
                {
                    case ExpanderState.Hide: return this.Hide;
                    case ExpanderState.FlyoutShow: return this._vsIsSecondPage ? this.FlyoutShowSecondPage : this.FlyoutShow;
                    case ExpanderState.OverlayNotExpanded: return this.OverlayNotExpanded;
                    case ExpanderState.Overlay: return this._vsIsSecondPage ? this.OverlaySecondPage : this.Overlay;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        Point _postion;


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

            this.Button.Self.Tapped += (s, e) => this.State = this.GetButtonState(this.State);

            this.CloseButton.Tapped += (s, e) => this.State = ExpanderState.Hide;
            this.StateButton.Tapped += (s, e) => this.State = this.GetState(this.State);

            this.BackButton.Tapped += (s, e) => this.IsSecondPage = false;
            if (this.Reset != null) this.ResetButton.Tapped += (s, e) => this.Reset();

            /////////////////////////////////

            //Postion 
            this.TitleGrid.ManipulationMode = ManipulationModes.All;
            this.TitleGrid.ManipulationStarted += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postion = VisualUIElementHelper.GetVisualPostion(this.Layout);
                this.Move?.Invoke(); //Delegate
            };
            this.TitleGrid.ManipulationDelta += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this._postion.X += e.Delta.Translation.X;
                this._postion.Y += e.Delta.Translation.Y;

                Point postion2 = VisualUIElementHelper.GetBoundPostion(this._postion, this.Layout);
                VisualUIElementHelper.SetOverlayPostion(this.Layout, postion2);
            };
            this.TitleGrid.ManipulationCompleted += (s, e) =>
            {
                this._postion = VisualUIElementHelper.GetVisualPostion(this.Layout);
            };
        }


        private ExpanderState GetState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.Overlay;
        }
        private ExpanderState GetButtonState(ExpanderState state)
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


        //Width
        private ExpanderWidth WidthMode
        {
            set
            {
                this.WidthFlyoutItem222.IsChecked = (value == ExpanderWidth.Width222);
                this.WidthFlyoutItem272.IsChecked = (value == ExpanderWidth.Width272);
                this.WidthFlyoutItem322.IsChecked = (value == ExpanderWidth.Width322);
                this.WidthFlyoutItem372.IsChecked = (value == ExpanderWidth.Width372);

                this.WidthFrame.Value = (int)value;
                this.WidthStoryboard.Begin();//Storyboard
            }
        }

        private void ConstructWidthStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.WidthKeyFrames, this.RootGrid);
            Storyboard.SetTargetProperty(this.WidthKeyFrames, "(UIElement.Width)");

            this.WidthFlyoutItem222.IsChecked = true;
            this.WidthFlyoutItem222.Click += (s, e) => this.WidthMode = ExpanderWidth.Width222;
            this.WidthFlyoutItem272.Click += (s, e) => this.WidthMode = ExpanderWidth.Width272;
            this.WidthFlyoutItem322.Click += (s, e) => this.WidthMode = ExpanderWidth.Width322;
            this.WidthFlyoutItem372.Click += (s, e) => this.WidthMode = ExpanderWidth.Width372;

            this.TitleGrid.RightTapped += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
            this.TitleGrid.Holding += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        }


        //Height
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


        public void HideLayout()
        {
            switch (this.State)
            {
                case ExpanderState.FlyoutShow:
                    this.State = ExpanderState.Hide;
                    break;
            }

            this.Layout.IsHitTestVisible = true;
        }

        public void CropLayout()
        {
            switch (this.State)
            {
                case ExpanderState.FlyoutShow:
                    this.State = ExpanderState.Hide;
                    break;
                case ExpanderState.Overlay:
                case ExpanderState.OverlayNotExpanded:
                    {
                        Point postion = VisualUIElementHelper.GetOverlayPostion(this.Layout);
                        Point postion2 = VisualUIElementHelper.GetBoundPostion(postion, this.Layout);
                        VisualUIElementHelper.SetOverlayPostion(this.Layout, postion2);
                    }
                    break;
            }
        }



    }
}