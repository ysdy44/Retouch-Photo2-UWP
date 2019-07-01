using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// On or off.
    /// </summary>
    public sealed partial class OnOffSwitch : UserControl
    { 
        //@Converter
        private SolidColorBrush FalseToBackgroundConverter(bool isOn) => (isOn == false) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush FalseToForegroundConverter(bool isOn) => (isOn == false) ? this.CheckColor : this.UnCheckColor;

        private SolidColorBrush TrueToBackgroundConverter(bool isOn) => (isOn == true) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush TrueToForegroundConverter(bool isOn) => (isOn == true) ? this.CheckColor : this.UnCheckColor;


        #region DependencyProperty


        /// <summary> Gets or sets whether the status of the <see cref = "OnOffSwitch" /> is "on". </summary>
        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.IsOn" /> dependency property. </summary>
        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(OnOffSwitch), new PropertyMetadata(false));


        /// <summary> Provides the object content that is displayed when the status of the <see cref = "OnOffSwitch" /> is on. </summary>
        public object OnContent
        {
            get { return (object)GetValue(OnContentProperty); }
            set { SetValue(OnContentProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.OnContent" /> dependency property. </summary>
        public static readonly DependencyProperty OnContentProperty = DependencyProperty.Register(nameof(OnContent), typeof(object), typeof(OnOffSwitch), new PropertyMetadata(null));


        /// <summary> Provides the object content that is displayed when the status of the <see cref = "OnOffSwitch" /> is off. </summary>
        public object OffContent
        {
            get { return (object)GetValue(OffContentProperty); }
            set { SetValue(OffContentProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.OffContent" /> dependency property. </summary>
        public static readonly DependencyProperty OffContentProperty = DependencyProperty.Register(nameof(OnContent), typeof(object), typeof(OnOffSwitch), new PropertyMetadata(null));


        #endregion


        //@Construct
        public OnOffSwitch()
        {
            this.InitializeComponent();
            this.OnSegmented.Tapped += (s, e) => this.IsOn = false;
            this.OffSegmented.Tapped += (s, e) => this.IsOn = true;
        }
    }
}