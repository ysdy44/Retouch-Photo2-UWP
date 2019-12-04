using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class Brush : ICacheTransform
    {
        /// <summary> <see cref="Brush">'s type. </summary>
        public BrushType Type;

        /// <summary> <see cref="Brush">'s color. </summary>
        public Color Color = Colors.Gray;

        /// <summary> <see cref="Brush">'s gradient colors. </summary>
        public CanvasGradientStop[] Array = GreyWhiteMeshHelpher.GetGradientStopArray();

        /// <summary> <see cref="Brush">'s points. </summary>
        public BrushPoints Points;
        private BrushPoints _startingPoints;
        

        /// <summary> <see cref = "Brush" />'s image. </summary>
        public ImageStr ImageStr { get; set; }
        
        /// <summary> The source transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination transformer. </summary>
        public Transformer ImageDestination { get; set; }
        Transformer _startingDestination;


        /// <summary>
        /// Copy a brush with self.
        /// </summary>
        /// <param name="brush"> The copyed brush. </param>
        public void CopyWith(Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None: break;
                case BrushType.Color:
                    this.Color = brush.Color;
                    break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.Points = brush.Points;
                    this.Array = (CanvasGradientStop[])brush.Array.Clone();
                    break;
                case BrushType.Image:
                    break;
            }
            this.Type = brush.Type;
        }

        /// <summary>
        /// Get Brush own copy.
        /// </summary>
        /// <returns> The cloned Brush. </returns>
        public Brush Clone()
        {
            Brush brush = new Brush
            {
                Type = this.Type,
            };

            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    brush.Color = this.Color;
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    brush.Array = (CanvasGradientStop[])this.Array.Clone();
                    brush.Points = this.Points;
                    brush._startingPoints = this._startingPoints;
                    break;

                case BrushType.Image:
                    brush.ImageStr = this.ImageStr;
                    brush.Source = this.Source;
                    brush.ImageDestination = this.ImageDestination;
                    break;
            }

            return brush;
        }
    }
}