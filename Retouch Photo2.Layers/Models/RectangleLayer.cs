using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s RectangleLayer .
    /// </summary>
    public class RectangleLayer : IGeometryLayer
    {
        //@Override      
        public override string Type => "Rectangle";
        public override UIElement Icon=> new RectangleIcon();


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            RectangleLayer rectangleLayer = new RectangleLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            base.CopyWith(resourceCreator, rectangleLayer);

            return rectangleLayer;
        }
    }
}