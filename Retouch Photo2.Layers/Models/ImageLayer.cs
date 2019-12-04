using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Image;


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


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }


        public IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromRectangle(transformer);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer = new ImageLayer();

            LayerBase.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
        }
        
        public void SaveWith(XElement element)
        {
        }
        public void Load(XElement element)
        {
        }

    }
}