using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Controls;
using Retouch_Photo2.Layers.ILayer;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s RectangleLayer .
    /// </summary>
    public class RectangleLayer : IGeometryLayer
    {
        //@Construct
        public RectangleLayer()
        {
            base.Name = "Rectangle";
        }

        //@Override      
        public override UIElement GetIcon()=> new RectangleControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new RectangleLayer
            {
                Name = this.Name,
                Opacity = this.Opacity,
                BlendType = this.BlendType,
                TransformerMatrix = this.TransformerMatrix,

                IsChecked = this.IsChecked,
                Visibility = this.Visibility,

                FillColor=base.FillColor,
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(base.TransformerMatrix.Destination.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(base.TransformerMatrix.Destination.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(base.TransformerMatrix.Destination.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(base.TransformerMatrix.Destination.LeftBottom, canvasToVirtualMatrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }
    }
}