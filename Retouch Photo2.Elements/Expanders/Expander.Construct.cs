using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Elements
{
    internal enum ExpanderWidth
    {
        Width222 = 222,
        Width272 = 272,
        Width322 = 322,
        Width372 = 372,
    }
    
    internal enum ExpanderHeight
    {
        Stretch,

        ZeroToMain,
        ZeroToSecond,

        MainToZero,
        SecondToZero,

        MainToSecond,
        SecondToMain,
    }

    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public partial class Expander : UserControl, IExpander
    {

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
        private ExpanderHeight HeightBegin
        {
            set
            {
                if (value == ExpanderHeight.Stretch)
                {
                    this.HeightRectangle.VerticalAlignment = VerticalAlignment.Stretch;
                    this.HeightRectangle.Height = double.NaN;
                }
                else
                {
                    this.HeightRectangle.VerticalAlignment = VerticalAlignment.Top;
                    this.HeightRectangle.Height = this.HeightRectangleHeight(value);

                    this.HeightFrame.Value = this.HeightFrameValue(value);
                    this.HeightStoryboard.Begin();//Storyboard
                }
            }
        }

        private double HeightRectangleHeight(ExpanderHeight height)
        {
            switch (height)
            {
                case ExpanderHeight.ZeroToSecond:
                case ExpanderHeight.ZeroToMain:
                    return 0;

                case ExpanderHeight.MainToZero:
                case ExpanderHeight.MainToSecond:
                    return this.MainPageHeight;

                case ExpanderHeight.SecondToZero:
                case ExpanderHeight.SecondToMain:
                    return this.SecondPageHeight;

                default: return 0;
            }
        }
        private double HeightFrameValue(ExpanderHeight height)
        {
            switch (height)
            {
                case ExpanderHeight.MainToZero:
                case ExpanderHeight.SecondToZero:
                    return 0;

                case ExpanderHeight.ZeroToMain:
                case ExpanderHeight.SecondToMain:
                    return this.MainPageHeight;

                case ExpanderHeight.ZeroToSecond:
                case ExpanderHeight.MainToSecond:
                    return this.SecondPageHeight;

                default: return 0;
            }
        }

        double MainPageHeight = 0;
        double SecondPageHeight = 0;
        private void ConstructHeightStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.HeightKeyFrames, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFrames, "(UIElement.Height)");

            this.MainPageBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.MainPageHeight = e.NewSize.Height;
            };
            this.SecondPageBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SecondPageHeight = e.NewSize.Height;

                this._lockLoaded();
            };
        }
        

        /// <summary> Is this Loaded? </summary>
        bool _lockIsLoaded = false;
        /// <summary>
        /// Loaded.
        /// </summary>
        private void _lockLoaded()
        {
            if (this._lockIsLoaded == false)
            {
                this._lockIsLoaded = true;

                this.HeightBegin = ExpanderHeight.MainToSecond;
            }
        }
        /// <summary>
        /// OnNavigatedTo.
        /// </summary>
        /// <param name="data"> The data. </param>
        private void _lockOnNavigatedTo(bool value)
        {
            this.HeightBegin = (value && this._lockIsLoaded) ? ExpanderHeight.MainToSecond : ExpanderHeight.SecondToMain;
        }

    }
}