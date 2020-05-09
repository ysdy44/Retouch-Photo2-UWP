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
        public Action Show { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }

        public Action<ExpanderState> StateChanged { get; set; }


        //@Content
        public string Title { set => this.TitleTextBlock.Text = value; get => this.TitleTextBlock.Text; }
        public FrameworkElement TitleGrid => this._TitleGrid;

        public FrameworkElement ResetButton => this._ResetButton;

        public UIElement MainPage { set => this.MainPageBorder.Child = value; get => this.MainPageBorder.Child; }
        public UIElement SecondPage { set => this.SecondPageBorder.Child = value; get => this.SecondPageBorder.Child; }


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
            set=>VisualStateManager.GoToState(this, value.Name, false);
        }
         
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                if (value == false)
                {
                    this._ResetButton.Visibility = Visibility.Collapsed;
                }

                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }
        

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
                            Point flyoutPostion = VisualUIElementHelper.GetFlyoutPostion(this.Button, this.Layout, placement);
                            Point boundPostion = VisualUIElementHelper.GetBoundPostion(flyoutPostion, this.Layout);
                            VisualUIElementHelper.SetOverlayPostion(this.Layout, boundPostion);
                            this.Move?.Invoke(); //Delegate
                            this.Show?.Invoke(); //Delegate

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

                this._vsState = value;
                this.VisualState = this.VisualState; //State
            }
        }

        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public Point Postion { get; set; }

        public FrameworkElement Layout;
        public FrameworkElement Button { get; set; } 


        //@Construct
        public Expander()
        {
            this.InitializeComponent();

            this.ConstructWidthStoryboard();
            this.ConstructHeightStoryboard();
            this.Tapped += (s, e) => e.Handled = true;
            this.Loaded += (s, e) =>
            {
                this.VisualState = this.VisualState;//State 

                if (this.Parent is FrameworkElement element)
                {
                    this.Layout = element;
                }
                else
                {
                    this.Layout = this;
                }
            };

            this._CloseButton.Tapped += (s, e) => this.StateChanged?.Invoke(ExpanderState.Hide);
            this._StateButton.Tapped += (s, e) => this.StateChanged?.Invoke(this.GetState(this.State));

            this._BackButton.Tapped += (s, e) => this.IsSecondPage = false;


            //Postion 
            this.TitleGrid.ManipulationMode = ManipulationModes.All;
            this.TitleGrid.ManipulationStarted += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                this.Postion = VisualUIElementHelper.GetVisualPostion(this.Layout);
                this.Move?.Invoke(); //Delegate
            };
            this.TitleGrid.ManipulationDelta += (s, e) =>
            {
                if (this.State == ExpanderState.FlyoutShow) return;

                Point point = this.Postion;
                point.X += e.Delta.Translation.X;
                point.Y += e.Delta.Translation.Y;
                this.Postion = point;

                Point postion2 = VisualUIElementHelper.GetBoundPostion(point, this.Layout);
                VisualUIElementHelper.SetOverlayPostion(this.Layout, postion2);
            };
            this.TitleGrid.ManipulationCompleted += (s, e) =>
            {
                this.Postion = VisualUIElementHelper.GetVisualPostion(this.Layout);
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

            this._TitleGrid.RightTapped += (s, e) => this.WidthMenuFlyout.ShowAt(this._TitleGrid);
            this._TitleGrid.Holding += (s, e) => this.WidthMenuFlyout.ShowAt(this._TitleGrid);
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

    }
}