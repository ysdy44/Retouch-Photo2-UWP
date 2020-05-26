using Retouch_Photo2.Brushs;
using Retouch_Photo2.Stroke;
using System.Xml.Linq;
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
        public Retouch_Photo2.Styles.Style Style2
        {
            get { return (Retouch_Photo2.Styles.Style)GetValue(Style2Property); }
            set { SetValue(Style2Property, value); }
        }
        /// <summary> Identifies the <see cref = "ShowControl.Style" /> dependency property. </summary>
        public static readonly DependencyProperty Style2Property = DependencyProperty.Register(nameof(Style2), typeof(Retouch_Photo2.Styles.Style), typeof(ShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            ShowControl con = (ShowControl)sender;

            if (e.NewValue is Retouch_Photo2.Styles.Style value)
            {
                con.Shape.Fill = value.Fill.ToBrush();

                con.Shape.Stroke = value.Stroke.ToBrush();

                float strokeWidth = value.StrokeWidth / 4.0f;
                if (strokeWidth > 5) strokeWidth = 5;
                con.Shape.StrokeThickness = strokeWidth;

                con.Shape.SetStrokeStyle(value.StrokeStyle);
            }
        }));

        #endregion


        //@Construct
        public ShowControl()
        {
            this.InitializeComponent();
        }

    }
}