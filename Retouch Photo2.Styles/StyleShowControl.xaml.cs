// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Stroke;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// A control used to show a style-style.
    /// </summary>
    public sealed partial class StyleShowControl : UserControl
    {

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "StyleShowControl" />'s style. </summary>
        public IStyle Style2
        {
            get => (IStyle)base.GetValue(Style2Property);
            set => base.SetValue(Style2Property, value);
        }
        /// <summary> Identifies the <see cref = "StyleShowControl.Style2" /> dependency property. </summary>
        public static readonly DependencyProperty Style2Property = DependencyProperty.Register(nameof(Style2), typeof(Retouch_Photo2.Styles.IStyle), typeof(StyleShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            StyleShowControl control = (StyleShowControl)sender;

            if (e.NewValue is IStyle value)
            {
                control.Shape.Fill = value.Fill.ToBrush();

                control.Shape.Stroke = value.Stroke.ToBrush();

                float strokeWidth = value.StrokeWidth / 4.0f;
                if (strokeWidth > 5) strokeWidth = 5;
                control.Shape.StrokeThickness = strokeWidth;

                control.Shape.SetStrokeStyle(value.StrokeStyle);
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a StyleShowControl. 
        /// </summary>
        public StyleShowControl()
        {
            this.InitializeComponent();
        }

    }
}