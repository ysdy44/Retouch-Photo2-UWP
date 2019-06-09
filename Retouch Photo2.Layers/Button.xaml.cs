using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Retouch_Photo2 Layers 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        public delegate void LayerTappedEventHandler(FrameworkElement placementTarget);

        //@Delegate
        /// <summary> Event of <see cref = "Button.RootGrid" />. </summary>
        public event LayerTappedEventHandler ItemClick;
        /// <summary> Event of <see cref = "Button.RootGrid" />. </summary>
        public event LayerTappedEventHandler FlyoutShow;

        /// <summary> Event of <see cref = "Button.IsVisualButton" />. </summary>
        public event TappedEventHandler IsVisualButtonTapped;
        /// <summary> Event of <see cref = "Button.CheckBox" />. </summary>
        public event TappedEventHandler CheckBoxTapped;
        

        //@Converter
        private double BoolToOpacityConverter(bool isChecked) => isChecked ? 1.0 : 0.4;
        private Visibility BoolToVisibilityConverter(bool isChecked) => isChecked ? Visibility.Visible : Visibility.Collapsed;
        

        #region DependencyProperty


        /// <summary> Gets or sets whether the <see cref = "Button" /> is selected. </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        /// <summary>  Identifies the <see cref = "Button.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(Button), new PropertyMetadata(false));


        /// <summary> Gets or sets whether the <see cref = "Button" /> is visual. </summary>
        public bool IsVisual
        {
            get { return (bool)GetValue(IsVisualProperty); }
            set { SetValue(IsVisualProperty, value); }
        }
        /// <summary>  Identifies the <see cref = "Button.IsVisual" /> dependency property. </summary>
        public static readonly DependencyProperty IsVisualProperty = DependencyProperty.Register(nameof(IsVisual), typeof(bool), typeof(Button), new PropertyMetadata(false));


        /// <summary> Gets or sets whether the <see cref = "Button" /> 's text. </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>  Identifies the <see cref = "Button.Text" /> dependency property. </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(Button), new PropertyMetadata(null));


        /// <summary> Gets or sets whether the <see cref = "Button" /> 's icon. </summary>
        public UIElement Icon
        {
            get { return (UIElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>  Identifies the <see cref = "Button.Icon" /> dependency property. </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(UIElement), typeof(Button), new PropertyMetadata(null));


        #endregion

        private static Button OnlyButton;

        public Button()
        {
            this.InitializeComponent();
            this.RootGrid.Tapped += (s, e) =>
            {
                if (Button.OnlyButton == this)  this.FlyoutShow?.Invoke(this);//Delegate
                else
                {
                    Button.OnlyButton = this;
                    this.ItemClick?.Invoke(this);//Delegate
                }
            };
            this.RootGrid.RightTapped += (s, e) => this.FlyoutShow?.Invoke(this);//Delegate
            this.RootGrid.Holding += (s, e) => this.FlyoutShow?.Invoke(this);//Delegate
            this.IsVisualButton.Tapped += (s, e) =>
            {
                this.IsVisualButtonTapped?.Invoke(s, e);//Delegate
                e.Handled = true;
            };
            this.CheckBox.Tapped += (s, e) =>
            {
                this.CheckBoxTapped?.Invoke(s, e);//Delegate
                e.Handled = true;
            };
        }
    }
}