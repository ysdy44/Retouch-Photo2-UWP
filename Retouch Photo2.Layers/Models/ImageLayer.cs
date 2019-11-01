using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Icons;
using System.Linq;
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

        //@Override     
        public override LayerType Type => LayerType.Image;

        //@Content
        /// <summary> <see cref = "ImageLayer" />'s image. </summary>
        public ImageStr ImageStr { get; set; }

        public CanvasBitmap CanvasBitmap
        {
            get
            {
                if (this.canvasBitmap == null)
                {
                    //Find the first image.
                    this.canvasBitmap = ImageRe.FindFirstImageRe(this.ImageStr).Source;
                }
                return this.canvasBitmap;
            }
        }
        private CanvasBitmap canvasBitmap;


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
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Text = "Image",
            };
        }


        public ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            if (base.StyleManager.FillBrush.Type == BrushType.None)
            {
                if (base.StyleManager.StrokeBrush.Type == BrushType.None)
                {
                    return this._getImage(canvasToVirtualMatrix);
                }
            }

            return new CompositeEffect
            {
                Sources =
                {
                    this._getRectangle(resourceCreator ,canvasToVirtualMatrix),
                    this._getImage(canvasToVirtualMatrix),
                }
            };
        }
        private ICanvasImage _getImage(Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 matrix = base.TransformManager.GetMatrix();

            return new Transform2DEffect
            {
                Source = this.CanvasBitmap,
                TransformMatrix = matrix * canvasToVirtualMatrix
            };
        }
        private ICanvasImage _getRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);

                //Fill
                this.StyleManager.FillGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
                //Stroke
                this.StyleManager.DrawGeometry(resourceCreator, drawingSession, geometry, canvasToVirtualMatrix);
            }
            return command;
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer= new ImageLayer
            {
                ImageStr = this.ImageStr,
            };

            LayerBase.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
        }


        public void SaveWith(XElement element)
        {
            element.Add(XML.SaveImageStr("ImageStr", this.ImageStr));
        }
        public void Load(XElement element)
        {
            this.ImageStr = XML.LoadImageStr(element.Element("ImageStr"));
        }

    }
}