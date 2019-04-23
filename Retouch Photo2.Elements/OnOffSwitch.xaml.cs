using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class OnOffSwitch : UserControl
    { 
        //Delegate
        public delegate void IsOnChangedHandler(bool isOn);
        public event IsOnChangedHandler IsOnChanged = null;

        public string OnContent
        {
            get => this.OnTextBlock.Text;
            set
            {
                this.OnTextBlock.Text = value;
                ToolTipService.SetToolTip(this.OnSegmented,value);
            }
        }
        public string OffCOfftent
        {
            get => this.OffTextBlock.Text;
            set
            {
                this.OffTextBlock.Text = value;
                ToolTipService.SetToolTip(this.OffSegmented, value);
            }
        }
        
        private bool _IsOn
        {
            set
            {
                this.SegmenteColor(this.OnSegmented, (value == false));
                this.SegmenteColor(this.OffSegmented, (value == true));

                this.IsOnChanged?.Invoke(value);//Delegate
            }
        }

        #region DependencyProperty


        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(OnOffSwitch), new PropertyMetadata(false, (sender, e) =>
        {
            OnOffSwitch con = (OnOffSwitch)sender;

            if (e.NewValue is bool value)
            {
                con._IsOn = value;
            }
        }));



        #endregion

        public OnOffSwitch()
        {
            this.InitializeComponent();
            this.Loaded += (sender, e) => this._IsOn = this.IsOn;
            this.OnSegmented.Tapped += (sender, e) => this.IsOn = false;
            this.OffSegmented.Tapped += (sender, e) => this.IsOn = true;
        }

        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}
