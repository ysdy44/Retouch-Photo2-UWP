// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Represents a control used to show a stroke-style.
    /// </summary>
    public sealed partial class StrokeShowControl : UserControl
    {
        
        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "StrokeShowControl" />'s stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle
        {
            get  => (CanvasStrokeStyle)base.GetValue(StrokeStyleProperty);
            set => base.SetValue(StrokeStyleProperty, value);
        }
        /// <summary> Identifies the <see cref = "StrokeShowControl.StrokeStyle" /> dependency property. </summary>
        public static readonly DependencyProperty StrokeStyleProperty = DependencyProperty.Register(nameof(StrokeStyle), typeof(CanvasStrokeStyle), typeof(StrokeShowControl), new PropertyMetadata(null, (sender, e) =>
        {
            StrokeShowControl control = (StrokeShowControl)sender;

            if (e.NewValue is CanvasStrokeStyle value)
            {
                control.Line.SetStrokeStyle(value);
            }
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a StrokeShowControl. 
        /// </summary>
        public StrokeShowControl()
        {
            this.InitializeComponent();
        }
        
    }
}