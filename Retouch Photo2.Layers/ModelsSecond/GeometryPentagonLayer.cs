using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPentagon;

        //@Content
        public int Points = 5;

        //@Construct   
        /// <summary>
        /// Construct a pentagon-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryPentagonLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a pentagon-layer.
        /// </summary>
        public GeometryPentagonLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryPentagonIcon(),
                Text = "Pentagon",
            };
        }
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, canvasToVirtualMatrix, this.Points);
        }


        public IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.ConvertToCurvesFromPentagon(transformer, this.Points);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPentagonLayer PentagonLayer = new GeometryPentagonLayer
            {
                Points = this.Points,
            };

            LayerBase.CopyWith(resourceCreator, PentagonLayer, this);
            return PentagonLayer;
        }

        public void SaveWith(XElement element)
        {
            element.Add(new XElement("Points", this.Points));
        }
        public void Load(XElement element)
        {
            this.Points = (int)element.Element("Points");
        }

    }
}