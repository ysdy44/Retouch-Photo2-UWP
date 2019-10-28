using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryRectangleLayer";

        //@Construct
        /// <summary>
        /// Construct a rectangle-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryRectangleLayer()
        {
            base.Type = GeometryRectangleLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRectangleIcon(),
                Text = "Rectangle",
            };
        }
        

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRectangleLayer rectangleLayer = new GeometryRectangleLayer();

            LayerBase.CopyWith(resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }


        public void SaveWith(XElement element) { }

    }
}