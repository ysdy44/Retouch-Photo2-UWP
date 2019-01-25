using System.Numerics;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Pages
{
    public sealed partial class MenuLayout : UserControl
    {
        //Postion
        Size ControlSize;
               
        private Vector2 postion;
        public Vector2 Postion
        {
            get => this.postion;
            set
            {
                Canvas.SetLeft(this, this.GetLeft(value.X));
                Canvas.SetTop(this, this.GetTop(value.Y));
            }
        }
        private double GetLeft(float X)
        {
            if (X < 0) return 0;
            if (this.ControlSize.Width > Window.Current.Bounds.Width) return 0;
            if (X > (Window.Current.Bounds.Width - this.ControlSize.Width)) return (Window.Current.Bounds.Width - this.ControlSize.Width);
            return X;
        }
        private double GetTop(float Y)
        {
            if (Y < 0 ) return 0;
            if (this.ControlSize.Height > Window.Current.Bounds.Height )return 0;
            if (Y > (Window.Current.Bounds.Height - this.ControlSize.Height)) return (Window.Current.Bounds.Height - this.ControlSize.Height);
            return Y;
        }


        //Label
        private bool label;
        public bool Label
        {
            get => label;
            set
            {
                this.LabelIcon.Glyph = value ? "\uE141" : "\uE196";
                this.ContentBorderl.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                label = value;
            }
        }

        //Content
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public UIElement CenterContent { get => this.ContentBorderl.Child; set => this.ContentBorderl.Child = value; }
        public UIElement FlyoutContent { get => this.FlyoutBorderl.Child; set => this.FlyoutBorderl.Child = value; }
        public Flyout Flyout { get => this.flyout; set => this.flyout = value; }
        public FlyoutPlacementMode Placement { get => this.flyout.Placement; set => this.flyout.Placement = value; }
 

        /// <summary>  显示相对于指定元素放置的浮出控件。</summary>
        /// <param name="placementTarget"> 要用作浮出控件位置目标的元素 </param>
        public void ShowAt(FrameworkElement placementTarget)
        {
            this.Visibility = Visibility.Collapsed;
            this.Flyout.ShowAt(placementTarget);
        }


        public MenuLayout()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) => this.Label = true;
            this.SizeChanged += (sender, e) => this.ControlSize = e.NewSize;

            //Postion 
            this.TitlePanel.ManipulationMode = ManipulationModes.All;
            this.TitlePanel.ManipulationStarted += (sender, e) => this.postion = new Vector2((float)Canvas.GetLeft(this), (float)Canvas.GetTop(this));
            this.TitlePanel.ManipulationDelta += (sender, e) => this.Postion = this.postion += e.Delta.Translation.ToVector2();
            this.TitlePanel.ManipulationCompleted += (sender, e) => { };

            //Label
            this.LabelButton.Tapped+=(sender, e) => this.Label = !this.Label;

            //Flyout
            this.PutButton.Tapped += (sender, e) =>
            {
                this.Visibility = Visibility.Visible;
                Point postion = this.FlyoutBorderl.TransformToVisual(Window.Current.Content).TransformPoint(new Point());
                Canvas.SetLeft(this, postion.X);
                Canvas.SetTop(this, postion.Y);
                if (!this.Label) this.Label = true;
                this.Flyout.Hide();
            };
        }
               

        //Content
        private void ContentBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ContentRectangleFrameHeight.Value = e.NewSize.Height;
            this.ContentRectangleStoryboard.Begin();
        }
    }
}
