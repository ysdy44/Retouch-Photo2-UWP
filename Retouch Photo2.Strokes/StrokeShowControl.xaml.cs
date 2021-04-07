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
        public CanvasStrokeStyle StrokeStyle { set => this.Line.SetStrokeStyle(value); }

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