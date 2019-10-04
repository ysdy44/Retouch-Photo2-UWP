using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : LayerBase
    {
        /// <summary> <see cref = "ImageLayer" />'s image. </summary>
        public ImageRe ImageRe { get; set; }

        //@Construct
        public ImageLayer(LayerCollection layerCollection) : base(layerCollection)
        {
            base.Control.Icon = new ImageIcon();
        }

        //@Override
        public override string Type => "Image";
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 matrix = base.TransformManager.GetMatrix();

            return new Transform2DEffect
            {
                Source = this.ImageRe.Source,
                TransformMatrix = matrix * canvasToVirtualMatrix
            };
        }

        public override ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer= new ImageLayer(layerCollection)
            {
                ImageRe = this.ImageRe,
            };

            LayerBase.CopyWith(layerCollection, resourceCreator, imageLayer, this);
            return imageLayer;
        }
    }
}