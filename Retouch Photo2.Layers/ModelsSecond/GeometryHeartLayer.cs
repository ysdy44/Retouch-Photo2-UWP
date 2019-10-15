using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : IGeometryLayer
    {
        //@Construct
        public GeometryHeartLayer()
        {
            base.Control.Icon = new GeometryHeartIcon();
            base.Control.Text = "Heart";
        }

        //@Override       
        public override string Type => "Heart";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryHeartLayer HeartLayer = new GeometryHeartLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, HeartLayer, this);
            return HeartLayer;
        }
    }
}