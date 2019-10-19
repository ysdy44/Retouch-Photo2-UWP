using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : IGeometryLayer, ILayer
    {
        //@Content       
        public string Type => "GeometryPentagonLayer";

        public int Points = 5;

        //@Construct
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
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;

            float rotation = GeometryUtil.StartingRotation;
            float angle = FanKit.Math.Pi * 2.0f / this.Points;

            Vector2[] points = new Vector2[this.Points];
            for (int i = 0; i < this.Points; i++)
            {
                int index = i;

                //Outer
                Vector2 outer = GeometryUtil.GetRotationVector(rotation);
                points[index] = Vector2.Transform(outer, matrix);
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
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

        public XElement Save()
        {
            XElement element = new XElement("GeometryPentagonLayer");

            element.Add(new XElement("Points", this.Points));

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}