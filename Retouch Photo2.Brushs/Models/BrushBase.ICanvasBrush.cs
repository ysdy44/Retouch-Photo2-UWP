using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase : IBrush
    {

        public ICanvasBrush GetICanvasBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None: return null;
                case BrushType.Color: return new CanvasSolidColorBrush(resourceCreator, this.Color);
            }


            Vector2 center = Vector2.Transform(this.Center, matrix);

            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

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
                        Photocopier photocopier = this.Photocopier;
                        if (photocopier.Name == null) return null;

                        Photo photo = Photo.FindFirstPhoto(photocopier);
                        CanvasBitmap bitmap = photo.Source;


                        float canvasWidth = (float)bitmap.Size.Width;
                        float canvasHeight = (float)bitmap.Size.Height;
                        Vector2 canvasCenter = new Vector2(canvasWidth / 2, canvasHeight / 2);

                        float radian = FanKit.Math.Pi + FanKit.Math.VectorToRadians(center - xPoint);
                        float xScale = Vector2.Distance(xPoint, center) / canvasWidth * 2;
                        float yScale = Vector2.Distance(yPoint, center) / canvasHeight * 2;

                        Matrix3x2 matrix2 =
                            Matrix3x2.CreateTranslation(-canvasCenter) *
                            Matrix3x2.CreateScale(xScale, yScale) *
                            Matrix3x2.CreateRotation(radian) *
                            Matrix3x2.CreateTranslation(center);


                        return new CanvasImageBrush(resourceCreator, bitmap)
                        {
                            Transform = matrix2,
                            ExtendX = this.Extend,
                            ExtendY = this.Extend,
                        };
                    }
                default:
                    return null;
            }
        }

    }
}