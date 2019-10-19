using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : LayerBase, ILayer
    {   
        //@Content
        public string Type => "ImageLayer";
        
        /// <summary> <see cref = "ImageLayer" />'s image. </summary>
        public ImageRe ImageRe { get; set; }

        //@Construct
        public ImageLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Text = "Image",
            };
        }

        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 matrix = base.TransformManager.GetMatrix();

            return new Transform2DEffect
            {
                Source = this.ImageRe.Source,
                TransformMatrix = matrix * canvasToVirtualMatrix
            };
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer= new ImageLayer
            {
                ImageRe = this.ImageRe,
            };

            LayerBase.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
        }

        public XElement Save()
        {
            XElement element = new XElement("ImageLayer");

            element.Add(new XElement("ImageRe", this.ImageRe));

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}