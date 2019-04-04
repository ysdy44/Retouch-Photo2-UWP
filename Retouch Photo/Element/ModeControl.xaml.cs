using Retouch_Photo.Library;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Element
{
    public sealed partial class ModeControl : UserControl
    {
        //Delegate
        public delegate void ModeChangedHandler(MarqueeMode mode);
        public event ModeChangedHandler ModeChanged = null;

        private MarqueeMode _Mode
        {
            set
            {
                this.SegmenteColor(this.NoneSegmented, (value == MarqueeMode.None));
                this.SegmenteColor(this.SquareSegmented, (value == MarqueeMode.Square));
                this.SegmenteColor(this.CenterSegmented, (value == MarqueeMode.Center));
                this.SegmenteColor(this.SquareAndCenterSegmented, (value == MarqueeMode.SquareAndCenter));

                this.ModeChanged?.Invoke(value);//Delegate
            }
        }

        #region DependencyProperty


        public MarqueeMode Mode
        {
            get { return (MarqueeMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(MarqueeMode), typeof(ModeControl), new PropertyMetadata(MarqueeMode.None, (sender, e) =>
        {
            ModeControl con = (ModeControl)sender;

            if (e.NewValue is MarqueeMode value)
            {
                con._Mode = value;
            }
        }));



        #endregion

        public ModeControl()
        {
            this.InitializeComponent();

            this.NoneSegmented.Tapped += (sender, e) => this._Mode = MarqueeMode.None;
            this.SquareSegmented.Tapped += (sender, e) => this._Mode = MarqueeMode.Square;
            this.CenterSegmented.Tapped += (sender, e) => this._Mode = MarqueeMode.Center;
            this.SquareAndCenterSegmented.Tapped += (sender, e) => this._Mode = MarqueeMode.SquareAndCenter;
        }

        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}
