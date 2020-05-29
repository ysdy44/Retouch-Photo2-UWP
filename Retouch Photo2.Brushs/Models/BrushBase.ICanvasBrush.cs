using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase : IBrush
    {

        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator)
        {
            switch (this.Type)
            {
                case BrushType.None: return null;
                case BrushType.Color: return new CanvasSolidColorBrush(resourceCreator, this.Color);
            }


            Vector2 center = this.Center;

            Vector2 xPoint = this.XPoint;
            Vector2 yPoint = this.YPoint;

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                    return new CanvasLinearGradientBrush(resourceCreator, this.Stops, this.Extend, CanvasAlphaMode.Premultiplied)
                    {
                        StartPoint = center,
                        EndPoint = yPoint,
                    };
                case BrushType.RadialGradient:
                    {
                        float radius = Vector2.Distance(center, yPoint);

                        return new CanvasRadialGradientBrush(resourceCreator, this.Stops, this.Extend, CanvasAlphaMode.Premultiplied)
                        {
                            RadiusX = radius,
                            RadiusY = radius,
                            Center = center
                        };
                    }
                case BrushType.EllipticalGradient:
                    {
                        float radiusX = Vector2.Distance(center, xPoint);
                        float radiusY = Vector2.Distance(center, yPoint);
                        float radian = FanKit.Math.Pi + FanKit.Math.VectorToRadians(xPoint - center);

                        Matrix3x2 matrix2 =
                            Matrix3x2.CreateTranslation(-center) *
                            Matrix3x2.CreateRotation(radian) *
                            Matrix3x2.CreateTranslation(center);

                        return new CanvasRadialGradientBrush(resourceCreator, this.Stops, this.Extend, CanvasAlphaMode.Premultiplied)
                        {
                            Transform = matrix2,
                            RadiusX = radiusX,
                            RadiusY = radiusY,
                            Center = center
                        };
                    }
                case BrushType.Image:
                    {
                        if (this.bitmap== null) return null;
                        
                        return new CanvasImageBrush(resourceCreator, this.bitmap)
                        {
                            Transform = this.GetimageMatrix(center, xPoint, yPoint, this.transformerRect),
                            ExtendX = this.Extend,
                            ExtendY = this.Extend,
                        };
                    }
                default:
                    return null;
            }
        }

        private Matrix3x2 GetimageMatrix(Vector2 center, Vector2 xPoint, Vector2 yPoint, FanKit.Transformers.TransformerRect transformerRect)
        {
            float xScale = Vector2.Distance(xPoint, center) / transformerRect.Width * 2;
            float yScale = Vector2.Distance(yPoint, center) / transformerRect.Height * 2;
            float radian = 0;


            if (yPoint.X == center.X && yPoint.Y < center.Y)
            {
                radian = -FanKit.Math.Pi;
            }
            else if (xPoint.X == center.X && xPoint.Y < center.Y)
            {
                radian = FanKit.Math.PiOver2 + FanKit.Math.Pi;
            }
            else
            {
                radian = FanKit.Math.Pi + FanKit.Math.VectorToRadians(center - xPoint);
            }


            return
                 Matrix3x2.CreateTranslation(-transformerRect.Center) *
                Matrix3x2.CreateScale(xScale, yScale) *
                Matrix3x2.CreateRotation(radian) *
                Matrix3x2.CreateTranslation(center);
        }

    }
}