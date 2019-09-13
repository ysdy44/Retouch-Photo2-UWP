using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Tools
{
    public sealed partial class TouchbarButton : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the IsChecked change. </summary>
        public EventHandler<bool> Toggled;

        //@Converter
        private SolidColorBrush BoolToBackgroundConverter(bool isChecked) => isChecked ? this.AccentColor : this.UnAccentColor;
        private SolidColorBrush BoolToForegroundConverter(bool isChecked) => isChecked ? this.CheckColor : this.UnCheckColor;

        #region DependencyProperty


        /// <summary> Gets or sets whether the status of the <see cref = "OnOffSwitch" /> is "on". </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "OnOffSwitch.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(TouchbarSlider), new PropertyMetadata(false));


        /// <summary> Get or set the string Unit for range elements. </summary>
        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }
        /// <summary> Identifies the <see cref = "NumberPicker.Unit" /> dependency property. </summary>
        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof(Unit), typeof(string), typeof(TouchbarSlider), new PropertyMetadata(string.Empty));


        /// <summary> Get or set the current value for a TouchbarButton. </summary>
        public int Number
        {
            get => this.number;
            set
            {
                this.TextBlock.Text = $"{value} {this.Unit}";
                this.number = value;
            }
        }
        private int number;


        #endregion
        
        //@Construct
        /// <summary>
        /// Construct a TouchbarButton.
        /// </summary>
        public TouchbarButton()
        {
            this.InitializeComponent();
            this.RootBorder.Tapped+=(s,e) => this.Toggled?.Invoke(this, this.IsChecked);//Delegate
        }
    }
}