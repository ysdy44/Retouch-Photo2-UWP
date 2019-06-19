using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Controls used to toggle checked states.
    /// </summary>
    public sealed partial class ToggleControl : UserControl
    {        
        //@Converter
        private SolidColorBrush FalseToBackgroundConverter(bool isOn) => (isOn == false) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush FalseToForegroundConverter(bool isOn) => (isOn == false) ? this.CheckColor : this.UnCheckColor;

        private SolidColorBrush TrueToBackgroundConverter(bool isOn) => (isOn == true) ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush TrueToForegroundConverter(bool isOn) => (isOn == true) ? this.CheckColor : this.UnCheckColor;

        //@Content
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
               
        #region DependencyProperty
        
        /// <summary> Gets or sets whether the status of the <see cref = "ToggleControl" /> is "on". </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ToggleControl.IsOn" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ToggleControl), new PropertyMetadata(false));

        #endregion

        //@Construct
        public ToggleControl()
        {
            this.InitializeComponent();
            this.RootGrid.Tapped += (s, e) => this.IsChecked = !this.IsChecked;
        }
    }
}