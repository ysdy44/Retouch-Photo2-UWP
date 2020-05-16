using Retouch_Photo2.Brushs;
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
        public Retouch_Photo2.Brushs.Style Style2
        {
            get { return (Brushs.Style)GetValue(Style2Property); }
            set { SetValue(Style2Property, value); }
        }
        /// <summary> Identifies the <see cref = "ShowControl.Style" /> dependency property. </summary>
        public static readonly DependencyProperty Style2Property = DependencyProperty.Register(nameof(Style2), typeof(Retouch_Photo2.Brushs.Style), typeof(ShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            ShowControl con = (ShowControl)sender;

            if (e.NewValue is Brushs.Style value)
            {
                con.Shape.Fill = value.FillBrush.ToBrush();
                con.Shape.Stroke = value.StrokeBrush.ToBrush();
                con.Shape.StrokeThickness = value.StrokeWidth;

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