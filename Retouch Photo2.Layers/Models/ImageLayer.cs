using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Image;


        /// <summary> <see cref = "ImageLayer" />'s photocopier. </summary>
        public Photocopier Photocopier { get; set; }


        //@Construct   
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        public ImageLayer()
        {
            base.Control = new LayerControl
            {
                Icon = new ImageIcon(),
                Type = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photocopier"> The fill photocopier. </param>
        public ImageLayer(Transformer transformer, Photocopier photocopier)
        {
            base.Control = new LayerControl
            {
                Icon = new ImageIcon(),
                Type = this.ConstructStrings(),
            };
            this.Photocopier = photocopier;
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer = new ImageLayer()
            {
                Photocopier = this.Photocopier,
            };

            Layer.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(Retouch_Photo2.Elements.XML.SavePhotocopier("Photocopier", this.Photocopier));
        }
        public override void Load(XElement element)
        {
            this.Photocopier = Retouch_Photo2.Elements.XML.LoadPhotocopier(element.Element("Photocopier"));
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix, IList<Layerage> children)
        {
            Photocopier photocopier = this.Photocopier;
            if (photocopier.Name == null) return null;

            Photo photo = Photo.FindFirstPhoto(photocopier);
            CanvasBitmap bitmap = photo.Source;

            Matrix3x2 matrix2 = base.Transform.GetMatrix();
            return new Transform2DEffect
            {
                TransformMatrix = matrix2 * canvasToVirtualMatrix,
                Source = bitmap,
                //TODO:  Cubic
                //InterpolationMode= CanvasImageInterpolation.Cubic
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromRectangle(transformer);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Image");
        }

        /// <summary>
        /// Convert to image brush.
        /// </summary>
        /// <returns> The product brush. </returns>
        public IBrush ToBrush()
        {
            Photocopier photocopier = this.Photocopier;
            Transformer transformer = this.Transform.Destination;
            return BrushBase.ImageBrush(transformer, photocopier);
        }

    }
}