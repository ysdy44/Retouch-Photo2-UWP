using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Controls;
using System.Numerics;
using Windows.UI.Xaml;

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
        public ImageLayer()
        {
            base.Name = "Image";
        }

        //@Override
        public override UIElement GetIcon() => new ImageControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            //@Debug
            CanvasBitmap bitmap = this.Bitmap;

            return new ImageLayer
            {
                Name = this.Name,
                Opacity = this.Opacity,
                BlendType = this.BlendType,
                TransformerMatrix = this.TransformerMatrix,

                IsChecked = this.IsChecked,
                Visibility = this.Visibility,

                Bitmap = bitmap,
            };
        }

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.Bitmap,
                TransformMatrix = base.TransformerMatrix.GetMatrix() * canvasToVirtualMatrix
            };
        }
    }
}