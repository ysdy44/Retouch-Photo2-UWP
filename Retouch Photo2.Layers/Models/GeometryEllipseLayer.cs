// Core:              ★★★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s GeometryEllipseLayer .
    /// </summary>
    public class GeometryEllipseLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryEllipse;


        public override ILayer Clone()
        {
            GeometryEllipseLayer ellipseLayer = new GeometryEllipseLayer();

            LayerBase.CopyWith(ellipseLayer, this);
            return ellipseLayer;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToEllipse(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToEllipse(resourceCreator, matrix);
        }

    }
}