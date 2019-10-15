using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryRoundRectLayer .
    /// </summary>
    public class GeometryRoundRectLayer : IGeometryLayer
    {
        public float Corner = 0.12f;

        //@Construct
        public GeometryRoundRectLayer()
        {
            base.Control.Icon = new GeometryRoundRectIcon();
            base.Control.Text = "RoundRect";
        }

        //@Override       
        public override string Type => "RoundRect";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRoundRectLayer RoundRectLayer = new GeometryRoundRectLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, RoundRectLayer, this);
            return RoundRectLayer;
        }
    }
}