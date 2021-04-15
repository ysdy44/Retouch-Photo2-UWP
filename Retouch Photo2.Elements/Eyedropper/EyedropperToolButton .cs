using HSVColorPickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// The <see cref="EyedropperToolButton"/> control helps use <see cref="Eyedropper"/> in view.
    /// </summary>
    public sealed partial class EyedropperToolButton : Button
    {
        //@Delegate
        /// <summary> Occurs when the color value changed. </summary>
        public event ColorChangeHandler ColorChanged;

        private readonly Eyedropper eyedropper = new Eyedropper();


        #region DependencyProperty

        /// <summary> Gets or sets the color. </summary>
        public Color Color
        {
            get => (Color)base.GetValue(ColorProperty);
            set => base.SetValue(ColorProperty, value);
        }
        /// Using a DependencyProperty as the backing store for <see cref="Eyedropper.Color"/>.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(EyedropperToolButton), new PropertyMetadata(Colors.Transparent));

        #endregion


        //@Construct
        /// <summary>
        /// Construct a StrawButton.
        /// </summary>
        public EyedropperToolButton()
        {
            //this.InitializeComponent();             
            this.Loaded += (s, e) =>
            {
                base.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
            };
            this.Unloaded += (s, e) =>
            {
                base.RemoveHandler(UIElement.PointerPressedEvent, new PointerEventHandler(Button_PointerPressed));
            };
        }

        private async void Button_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Color = await this.eyedropper.OpenAsync(e);
            this.ColorChanged?.Invoke(this, this.Color);
        }

    }
}