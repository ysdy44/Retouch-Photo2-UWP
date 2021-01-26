using Retouch_Photo2.Brushs;
using Retouch_Photo2.Stroke;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// A control used to show a style-style.
    /// </summary>
    public sealed partial class ShowControl : UserControl
    {

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "ShowControl" />'s style. </summary>
        public Retouch_Photo2.Styles.IStyle Style2
        {
            get => (Retouch_Photo2.Styles.IStyle)base.GetValue(Style2Property);
            set => base.SetValue(Style2Property, value);
        }
        /// <summary> Identifies the <see cref = "ShowControl.Style2" /> dependency property. </summary>
        public static readonly DependencyProperty Style2Property = DependencyProperty.Register(nameof(Style2), typeof(Retouch_Photo2.Styles.IStyle), typeof(ShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            ShowControl control = (ShowControl)sender;

            if (e.NewValue is Retouch_Photo2.Styles.IStyle value)
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
        /// Initializes a ShowControl. 
        /// </summary>
        public ShowControl()
        {
            this.InitializeComponent();
        }

    }
}