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
                this.WidthFlyoutItem222.IsChecked = false;
                this.WidthFlyoutItem272.IsChecked = false;
                this.WidthFlyoutItem322.IsChecked = false;
                this.WidthFlyoutItem372.IsChecked = false;

                switch (value)
                {
                    case ExpanderWidth.Width222:
                        this.WidthFlyoutItem222.IsChecked = true;
                        this.WidthStoryboard222.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width272:
                        this.WidthFlyoutItem272.IsChecked = true;
                        this.WidthStoryboard272.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width322:
                        this.WidthFlyoutItem322.IsChecked = true;
                        this.WidthStoryboard322.Begin();//Storyboard
                        break;
                    case ExpanderWidth.Width372:
                        this.WidthFlyoutItem372.IsChecked = true;
                        this.WidthStoryboard372.Begin();//Storyboard
                        break;
                }
            }
        }

        private void ConstructWidthStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.WidthKeyFrames222, this.RootGrid);
            Storyboard.SetTargetProperty(this.WidthKeyFrames222, "(UIElement.Width)");
            Storyboard.SetTarget(this.WidthStoryboard272, this.RootGrid);
            Storyboard.SetTargetProperty(this.WidthKeyFrames272, "(UIElement.Width)");
            Storyboard.SetTarget(this.WidthKeyFrames322, this.RootGrid);
            Storyboard.SetTargetProperty(this.WidthKeyFrames322, "(UIElement.Width)");
            Storyboard.SetTarget(this.WidthKeyFrames372, this.RootGrid);
            Storyboard.SetTargetProperty(this.WidthKeyFrames372, "(UIElement.Width)");

            this.WidthFlyoutItem222.IsChecked = true;
            this.WidthFlyoutItem222.Click += (s, e) => this.WidthMode = ExpanderWidth.Width222;
            this.WidthFlyoutItem272.Click += (s, e) => this.WidthMode = ExpanderWidth.Width272;
            this.WidthFlyoutItem322.Click += (s, e) => this.WidthMode = ExpanderWidth.Width322;
            this.WidthFlyoutItem372.Click += (s, e) => this.WidthMode = ExpanderWidth.Width372;

            this.TitleGrid.RightTapped += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
            this.TitleGrid.Holding += (s, e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        }


        //Height        
        private void ConstructHeightStoryboard()
        {
            // Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.HeightKeyFramesZeroToMain, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesZeroToMain, "(UIElement.Height)");
            Storyboard.SetTarget(this.HeightKeyFramesZeroToSecond, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesZeroToSecond, "(UIElement.Height)");
            Storyboard.SetTarget(this.HeightKeyFramesMainToZero, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesMainToZero, "(UIElement.Height)");
            Storyboard.SetTarget(this.HeightKeyFramesSecondToZero, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesSecondToZero, "(UIElement.Height)");
            Storyboard.SetTarget(this.HeightKeyFramesMainToSecond, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesMainToSecond, "(UIElement.Height)");
            Storyboard.SetTarget(this.HeightKeyFramesSecondToMain, this.HeightRectangle);
            Storyboard.SetTargetProperty(this.HeightKeyFramesSecondToMain, "(UIElement.Height)");

            this.HeightKeyFramesZeroToMain.From = 0;
            this.HeightKeyFramesZeroToSecond.From = 0;
            this.HeightKeyFramesMainToZero.To = 0;
            this.HeightKeyFramesSecondToZero.To = 0;

            this.MainPageBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                double height = e.NewSize.Height;
                this.HeightKeyFramesZeroToMain.To = height;
                this.HeightKeyFramesMainToZero.From = height;
                this.HeightKeyFramesMainToSecond.From = height;
                this.HeightKeyFramesSecondToMain.To = height;
            };
            this.SecondPageBorder.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                double height = e.NewSize.Height;
                this.HeightKeyFramesZeroToSecond.From = height;
                this.HeightKeyFramesSecondToZero.To = height;
                this.HeightKeyFramesMainToSecond.To = height;
                this.HeightKeyFramesSecondToMain.From = height;
            };
        }
    }
}