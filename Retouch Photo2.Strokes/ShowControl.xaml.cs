using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Stroke;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Represents a control used to show a stroke-style.
    /// </summary>
    public sealed partial class ShowControl : UserControl
    {
        
        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "ShowControl" />'s stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle
        {
            get  => (CanvasStrokeStyle)base.GetValue(StrokeStyleProperty);
            set => base.SetValue(StrokeStyleProperty, value);
        }
        /// <summary> Identifies the <see cref = "ShowControl.StrokeStyle" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeStyleProperty = DependencyProperty.Register(nameof(StrokeStyle), typeof(CanvasStrokeStyle), typeof(ShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            ShowControl control = (ShowControl)sender;

            if (e.NewValue is CanvasStrokeStyle value)
            {
                control.Line.SetStrokeStyle(value);
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