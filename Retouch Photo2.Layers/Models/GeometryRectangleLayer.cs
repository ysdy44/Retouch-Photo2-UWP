using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRectangle;

        //@Construct
        /// <summary>
        /// Initializes a rectangle-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryRectangleLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRectangleIcon(),
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRectangleLayer rectangleLayer = new GeometryRectangleLayer();

            LayerBase.CopyWith(resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.ConvertToCurvesFromRectangle(transformer);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryRectangle");
        }

    }
}