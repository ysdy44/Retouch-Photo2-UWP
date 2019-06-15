using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Retouch_Photo2 Layers 's GroupButton.
    /// </summary>
    public sealed partial class GroupButton : UserControl
    {
         //@Delegate
        /// <summary> Event of <see cref = "Button.VisibilityButton" />. </summary>
        public event TappedEventHandler VisibilityButtonTapped;

        
        //@Converter
        private double VisibilityToOpacityConverter(Visibility visibility) => (visibility == Visibility.Visible) ? 1.0 : 0.4;
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
        public Visibility Visibility
        {
            get { return (Visibility)GetValue(IsVisualProperty); }
            set { SetValue(IsVisualProperty, value); }
        }
        /// <summary>  Identifies the <see cref = "Button.Visibility" /> dependency property. </summary>
        public static readonly DependencyProperty IsVisualProperty = DependencyProperty.Register(nameof(Visibility), typeof(Visibility), typeof(Button), new PropertyMetadata(Visibility.Visible));


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

        public GroupButton()
        {
            this.InitializeComponent();
            this.VisibilityButton.Tapped += (s, e) =>
            {
                this.VisibilityButtonTapped?.Invoke(s, e);//Delegate
                e.Handled = true;
            };
        }
    }
}
