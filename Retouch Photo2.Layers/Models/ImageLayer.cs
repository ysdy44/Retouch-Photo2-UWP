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
        //@Static     
        public const string ID = "ImageLayer";
         
        //@Content
        /// <summary> <see cref = "ImageLayer" />'s image. </summary>
        public ImageRe ImageRe { get; set; }

        //@Construct   
        /// <summary>
        /// Construct a image-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public ImageLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a image-layer.
        /// </summary>
        public ImageLayer()
        {
            base.Type = ImageLayer.ID;
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

            //TODO: ImageRe
            //element.Add(new XElement("ImageRe", this.ImageRe));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            //TODO: ImageRe
           // this.BlurAmount = (float)element.Descendants("BlurAmount").Single();
            LayerBase.LoadWith(element, this);
        }

    }
}