using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Controls;
using System.Numerics;
using Windows.Graphics.Effects;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : Layer
    {
        /// <summary> <see cref = "ImageLayer" />'s bitmap. </summary>
        public CanvasBitmap Bitmap { set; get; }

        //@Construct
        /// <summary> Construct a ImageLayer. </summary>
        public ImageLayer()
        {
            base.Name = "Image";
            base.Icon = new ImageControl();
        }

        //@Override
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.Bitmap,
                TransformMatrix = base.Transformer.Matrix * canvasToVirtualMatrix
            };
        }
    }
}