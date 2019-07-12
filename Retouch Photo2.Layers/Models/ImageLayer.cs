using FanKit.Transformers;
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
        /// <summary> <see cref = "ImageLayer" />'s image. </summary>
        public ImageRe ImageRe { get; set; }

        //@Construct
        public ImageLayer()
        {
        }

        //@Override
        public override string Type => "Image";
        public override UIElement Icon => new ImageControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new ImageLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                ImageRe = this.ImageRe,
            };
        }
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            return new Transform2DEffect
            {
                Source = this.ImageRe.Source,
                TransformMatrix = base.GetMatrix() * canvasToVirtualMatrix
            };
        }
    }
}