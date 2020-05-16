using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// A control used to show a stroke-style.
    /// </summary>
    public sealed partial class ShowControl : UserControl
    {
        
        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "ShowControl" />'s stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle
        {
            get { return (CanvasStrokeStyle)GetValue(StrokeStyleProperty); }
            set { SetValue(StrokeStyleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ShowControl.StrokeStyle" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeStyleProperty = DependencyProperty.Register(nameof(StrokeStyle), typeof(CanvasStrokeStyle), typeof(ShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            ShowControl con = (ShowControl)sender;

            if (e.NewValue is CanvasStrokeStyle value)
            {
                con.Line.SetStrokeStyle(value);
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