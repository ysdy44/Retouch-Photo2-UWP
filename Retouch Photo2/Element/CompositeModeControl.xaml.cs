using Retouch_Photo2.Library;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class CompositeModeControl : UserControl
    {
        //Delegate
        public delegate void ModeChangedHandler(MarqueeCompositeMode mode);
        public event ModeChangedHandler ModeChanged = null;

        private MarqueeCompositeMode _Mode
        {
            set
            {
                this.SegmenteColor(this.NewSegmented, (value == MarqueeCompositeMode.New));
                this.SegmenteColor(this.AddSegmented, (value == MarqueeCompositeMode.Add));
                this.SegmenteColor(this.SubtractSegmented, (value == MarqueeCompositeMode.Subtract));
                this.SegmenteColor(this.IntersectSegmented, (value == MarqueeCompositeMode.Intersect));

                this.ModeChanged?.Invoke(value); //Delegate
            }
        }

        #region DependencyProperty


        public MarqueeCompositeMode Mode
        {
            get { return (MarqueeCompositeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeCompositeMode), typeof(CompositeModeControl), new PropertyMetadata(MarqueeCompositeMode.New, (sender, e) =>
        {
            CompositeModeControl con = (CompositeModeControl)sender;

            if (e.NewValue is MarqueeCompositeMode value)
            {
                con._Mode=value;
            }
        }));


        #endregion
        
        public CompositeModeControl()
        {
            this.InitializeComponent();

            this.NewSegmented.Tapped += (sender, e) => this.Mode = MarqueeCompositeMode.New;
            this.AddSegmented.Tapped += (sender, e) => this.Mode = MarqueeCompositeMode.Add;
            this.SubtractSegmented.Tapped += (sender, e) => this.Mode = MarqueeCompositeMode.Subtract;
            this.IntersectSegmented.Tapped += (sender, e) => this.Mode = MarqueeCompositeMode.Intersect;
        }

        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}
