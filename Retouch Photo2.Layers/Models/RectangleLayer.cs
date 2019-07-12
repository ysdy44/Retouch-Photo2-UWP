using FanKit.Transformers;
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
        }

        //@Override      
        public override string Type => "Rectangle";
        public override UIElement Icon=> new RectangleControl();
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {
            return new RectangleLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(base.Destination.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(base.Destination.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(base.Destination.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(base.Destination.LeftBottom, canvasToVirtualMatrix);

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