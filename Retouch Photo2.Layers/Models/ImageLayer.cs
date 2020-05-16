using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="ILayer"/>'s ImageLayer .
    /// </summary>
    public class ImageLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Image;


        //@Construct   
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        public ImageLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Text = this.ConstructStrings(),
            };
        }
        /// <summary>
        /// Initializes a image-layer.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photocopier"> The fill photocopier. </param>
        public ImageLayer(Transformer transformer, Photocopier photocopier)
        {
            base.Control = new LayerControl(this)
            {
                Icon = new ImageIcon(),
                Text = this.ConstructStrings(),
            };

            base.Style = new Style
            {
                Fill = new ImageBrush(transformer)
                {
                    Photocopier = photocopier
                }
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            ImageLayer imageLayer = new ImageLayer();

            LayerBase.CopyWith(resourceCreator, imageLayer, this);
            return imageLayer;
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

    }
}