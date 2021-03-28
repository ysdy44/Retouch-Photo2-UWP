// Core:              ★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★★
// Complete:      ★
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a ellipse used to display a color.
    /// </summary>
    public sealed partial class ColorEllipse : Control
    {

        #region DependencyProperty


        /// <summary> Gets or sets the brush. </summary>
        public IBrush Brush
        {
            get => (IBrush)base.GetValue(BrushProperty);
            set => base.SetValue(BrushProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorEllipse.Brush" /> dependency property. </summary>
        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush), typeof(IBrush), typeof(ColorEllipse), new PropertyMetadata(null, (sender, e) =>
        {
            ColorEllipse control = (ColorEllipse)sender;

            if (e.NewValue is IBrush value)
            {
                if (value.Type == BrushType.Color)
                {
                    control.Color = value.Color;
                }
            }
        }));


        /// <summary> Gets or sets the color. </summary>
        public Color Color
        {
            get => (Color)base.GetValue(ColorProperty);
            set => base.SetValue(ColorProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorEllipse.Color" /> dependency property. </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorEllipse), new PropertyMetadata(Colors.LightGray, (sender, e) =>
        {
            ColorEllipse control = (ColorEllipse)sender;

            if (e.NewValue is Color value)
            {
                control.SolidColorBrush = new SolidColorBrush(value);
            }
        }));


        /// <summary> Gets or sets the SolidColorBrush. </summary>
        public SolidColorBrush SolidColorBrush
        {
            get => (SolidColorBrush)base.GetValue(SolidColorBrushProperty);
            set => base.SetValue(SolidColorBrushProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorEllipse.Color" /> dependency property. </summary>
        public static readonly DependencyProperty SolidColorBrushProperty = DependencyProperty.Register(nameof(SolidColorBrush), typeof(SolidColorBrush), typeof(ColorEllipse), new PropertyMetadata(null));


        #endregion

        //@Construct
        /// <summary>
        /// Initializes a ColorEllipse. 
        /// </summary>
        public ColorEllipse()
        {
            this.DefaultStyleKey = typeof(ColorEllipse);
        }

    }
}