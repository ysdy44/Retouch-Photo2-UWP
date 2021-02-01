// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★
using HSVColorPickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a transparency.
    /// </summary>
    public sealed partial class TransparencyShowControl : UserControl
    {
        
        #region DependencyProperty


        /// <summary> Gets or sets the transparency. </summary>
        public IBrush Transparency
        {
            set
            {
                this._vsTransparency = value;
                this.Invalidate();//Invalidate
            }
        }


        #endregion

        //@VisualState
        IBrush _vsTransparency;
        /// <summary>
        /// Invalidate.
        /// </summary>
        public void Invalidate()
        {
            this.Rectangle.Fill = this.ToBrush(this._vsTransparency);
        }


        //@Construct
        /// <summary>
        /// Initializes a TransparencyShowControl. 
        /// </summary>
        public TransparencyShowControl()
        {
            this.InitializeComponent();
        }

        private Brush ToBrush(IBrush brush)
        {
            if (brush == null) return this.NoneBrush;

            switch (brush.Type)
            {
                case BrushType.None: return this.NoneBrush;

                case BrushType.LinearGradient:
                    this.LinearGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.LinearGradientBrush;

                case BrushType.RadialGradient:
                    this.RadialGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.RadialGradientBrush;

                case BrushType.EllipticalGradient:
                    this.EllipticalGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.EllipticalGradientBrush;

                default: return this.NoneBrush;
            }
        }

    }
}