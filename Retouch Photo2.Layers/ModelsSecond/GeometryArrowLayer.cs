using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s ArrowLayer .
    /// </summary>
    public class GeometryArrowLayer : IGeometryLayer
    {
        //@Construct
        public GeometryArrowLayer()
        {
            base.Control.Icon = new GeometryArrowIcon();
            base.Control.Text = "Arrow";
        }

        //@Override       
        public override string Type => "Arrow";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryArrowLayer ArrowLayer = new GeometryArrowLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, ArrowLayer, this);
            return ArrowLayer;
        }
    }
}