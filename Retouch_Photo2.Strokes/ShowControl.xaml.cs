using Microsoft.Graphics.Canvas.Geometry;
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
                con.Line.StrokeDashArray = con.GetDashStyle(value.DashStyle);
                con.Line.StrokeDashCap = con.GetCapStyle(value.DashCap);
                con.Line.StrokeStartLineCap = con.GetCapStyle(value.StartCap);
                con.Line.StrokeEndLineCap = con.GetCapStyle(value.EndCap);
                con.Line.StrokeDashOffset = value.DashOffset;
            }
        }));

        #endregion


        //@Construct
        public ShowControl()
        {
            this.InitializeComponent();
        }


        private PenLineCap GetCapStyle(CanvasCapStyle cap)
        {
            switch (cap)
            {
                case CanvasCapStyle.Flat: return PenLineCap.Flat;
                case CanvasCapStyle.Square: return PenLineCap.Square;
                case CanvasCapStyle.Round: return PenLineCap.Round;
                case CanvasCapStyle.Triangle: return PenLineCap.Triangle;
                default: return PenLineCap.Flat;
            }
        }

        private DoubleCollection GetDashStyle(CanvasDashStyle style)
        {
            switch (style)
            {
                case CanvasDashStyle.Solid: return null;
                case CanvasDashStyle.Dash: return new DoubleCollection { 2, 2 };
                case CanvasDashStyle.Dot: return new DoubleCollection { 0, 2 };
                case CanvasDashStyle.DashDot: return new DoubleCollection { 2, 2, 0, 2 };
                case CanvasDashStyle.DashDotDot: return new DoubleCollection { 2, 2, 0, 2, 0, 2 };
                default: return null;
            }
        }
        
    }
}