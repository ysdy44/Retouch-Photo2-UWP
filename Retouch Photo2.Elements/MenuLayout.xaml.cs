using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// State of <see cref="MenuLayout"/>.
    /// </summary>
    internal enum MenuLayoutState
    {
        /// <summary> Flyout showed. </summary>
        Flyout,
        /// <summary> Root expanded. </summary>
        RootExpanded,
        /// <summary> Root not expanded. </summary>
        RootNotExpanded
    }


    /// <summary>
    /// MenuLayout:
    /// A layout control with Flyout and Overlay Layout.
    /// It can be moved on the screen.
    /// </summary>
    public sealed partial class MenuLayout : UserControl
    {
        //@static
        public static void ShowFlyoutAt(MenuLayout layout, FrameworkElement placementTarget)
        {
            layout.State = MenuLayoutState.Flyout;
            layout.Flyout.ShowAt(placementTarget);
        }
        public static void LayoutBinging(MenuLayout layout, ToggleButton button)
        {
            layout.Flyout.Opened += (s, e) => button.IsChecked = true;
            layout.Flyout.Closed += (s, e) => button.IsChecked = false;
            MenuLayout.TappedBinging(layout, button);
        }
        public static void TappedBinging(MenuLayout layout, FrameworkElement element) => element.Tapped += (s, e) => MenuLayout.ShowFlyoutAt(layout, element);

        //Postion: the position of the Root on the canvas.
        private Size ControlSize;
        private Vector2 Postion;
        private Vector2 GetElementVisualPostion(UIElement element) => element.TransformToVisual(Window.Current.Content).TransformPoint(new Point()).ToVector2();
        private Vector2 GetElementCanvasPostion(UIElement element) => new Vector2((float)Canvas.GetLeft(element), (float)Canvas.GetTop(element));
        private void SetElementCanvasPostion(UIElement element, Vector2 postion, Size size)
        {
            double X;
            if (postion.X < 0) X = 0;
            else if (size.Width > Window.Current.Bounds.Width) X = 0;
            else if (postion.X > (Window.Current.Bounds.Width - size.Width)) X = (Window.Current.Bounds.Width - size.Width);
            else X = postion.X;
            Canvas.SetLeft(element, X);

            double Y;
            if (postion.Y < 0) Y = 0;
            else if (size.Height > Window.Current.Bounds.Height) Y = 0;
            else if (postion.Y > (Window.Current.Bounds.Height - size.Height)) Y = (Window.Current.Bounds.Height - size.Height);
            else Y = postion.Y;
            Canvas.SetTop(element, Y);
        }

        //State: content in the Flyout or the Root?
        private MenuLayoutState State
        {
            get => state;
            set
            {
                this.StateIcon.Glyph = (value == MenuLayoutState.Flyout) ? "\uE1CB" : (value == MenuLayoutState.RootExpanded) ? "\uE141" : "\uE196";
                this.ContentBorder.Visibility = (value == MenuLayoutState.RootNotExpanded) ? Visibility.Collapsed : Visibility.Visible;
                this.Visibility = this.StoryboardRectangle.Visibility = (value == MenuLayoutState.Flyout) ? Visibility.Collapsed : Visibility.Visible;

                if (value == MenuLayoutState.Flyout) //Content in the Flyout.
                {
                    this.RootBorder.Child = null;
                    this.FlyoutBorder.Child = this.ContentGrid;
                }
                else //Content in the Root.
                {
                    this.FlyoutBorder.Child = null;
                    this.RootBorder.Child = this.ContentGrid;
                }

                state = value;
            }
        }
        private MenuLayoutState state;

        //Content
        public string Text { set => this.TextBlock.Text = value; get => this.TextBlock.Text; }
        public UIElement Icon { set => this.IconViewBox.Child = value; get => this.IconViewBox.Child; }
        public UIElement ContentChild { set => this.ContentBorder.Child = value; get => this.ContentBorder.Child; }
        public FlyoutPlacementMode Placement { set => this.Flyout.Placement = value; get => this.Flyout.Placement; }

        public MenuLayout()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) => this.ControlSize = e.NewSize;

            //State
            this.State = MenuLayoutState.Flyout;
            this.StateButton.Tapped += (s, e) =>
            {
                if (this.State == MenuLayoutState.RootExpanded) this.State = MenuLayoutState.RootNotExpanded;
                else if (this.State == MenuLayoutState.RootNotExpanded) this.State = MenuLayoutState.RootExpanded;
                else
                {
                    Vector2 postion = this.GetElementVisualPostion(this.TitlePanel);
                    this.SetElementCanvasPostion(this, postion, this.ControlSize);

                    this.Flyout.Hide();
                    this.State = MenuLayoutState.RootExpanded;
                }
            };

            //Postion 
            this.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.TitlePanel.ManipulationStarted += (s, e) => this.Postion = this.GetElementCanvasPostion(this);
            this.TitlePanel.ManipulationDelta += (s, e) =>
            {
                this.Postion += e.Delta.Translation.ToVector2();
                this.SetElementCanvasPostion(this, this.Postion, this.ControlSize);
            };

            //Storyboard
            this.StoryboardBorder.SizeChanged += (s, e) =>
            {
                if (this.State == MenuLayoutState.Flyout) return;

                this.Frame.Value = e.NewSize.Height;
                this.Storyboard.Begin();
            };
        }
    }
}

