using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Brush Classes.
    /// </summary>
    public class Brush
    {
        /// <summary> <see cref="Brush">'s IsFollowTransform. </summary>
        public bool IsFollowTransform = true;

        /// <summary> <see cref="Brush">'s type. </summary>
        public BrushType Type;


        /// <summary> <see cref="Brush">'s color. </summary>
        public Color Color = Colors.Gray;
        /// <summary> <see cref="Brush">'s gradient colors. </summary>
        public CanvasGradientStop[] Array = new CanvasGradientStop[]
        {
             new CanvasGradientStop{Color= Colors.White, Position=0.0f },
             new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };


        /// <summary> <see cref="Brush">'s linear gradient start point. </summary>
        public Vector2 LinearGradientStartPoint;
        /// <summary> <see cref="Brush">'s linear gradient end point. </summary>
        public Vector2 LinearGradientEndPoint;

        /// <summary> <see cref="Brush">'s radial gradient center point. </summary>
        public Vector2 RadialGradientCenter;
        /// <summary> <see cref="Brush">'s radial gradient control point. </summary>
        public Vector2 RadialGradientPoint;

        /// <summary> <see cref="Brush">'s elliptical gradient center point. </summary>
        public Vector2 EllipticalGradientCenter;
        /// <summary> <see cref="Brush">'s elliptical gradient x-point. </summary>
        public Vector2 EllipticalGradientXPoint;
        /// <summary> <see cref="Brush">'s elliptical gradient y-point. </summary>
        public Vector2 EllipticalGradientYPoint;


        /// <summary> <see cref="Brush">'s CanvasImageBrush. </summary>
        public CanvasImageBrush ImageBrush;



        /// <summary> Get radians of the vector in the coordinate system. </summary>
        public float VectorToRadians(Vector2 vector)
        {
            float tan = (float)Math.Atan(Math.Abs(vector.Y / vector.X));

            //First Quantity
            if (vector.X > 0 && vector.Y > 0) return tan;
            //Second Quadrant
            else if (vector.X > 0 && vector.Y < 0) return -tan;
            //Third Quadrant  
            else if (vector.X < 0 && vector.Y > 0) return (float)Math.PI - tan;
            //Fourth Quadrant  
            else return tan - (float)Math.PI;
        }
        /// <summary>
        /// Fill geometry.
        /// </summary>
        /// <param name="resourceCreator"> ICanvasResourceCreator. </param>
        /// <param name="ds"> CanvasDrawingSession. </param>
        /// <param name="geometry"> CanvasGeometry. </param>
        /// <param name="matrix"> matrix. </param>
        public void FillGeometry(ICanvasResourceCreator resourceCreator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    ds.FillGeometry(geometry, this.Color);
                    break;

                case BrushType.LinearGradient:
                    {
                        Vector2 startPoint = Vector2.Transform(this.LinearGradientStartPoint, matrix);
                        Vector2 endPoint = Vector2.Transform(this.LinearGradientEndPoint, matrix);

                        ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.Array)
                        {
                            StartPoint = startPoint,
                            EndPoint = endPoint,
                        };

                        ds.FillGeometry(geometry, brush);
                    }
                    break;

                case BrushType.RadialGradient:
                    {
                        Vector2 center = Vector2.Transform(this.RadialGradientCenter, matrix);
                        Vector2 point = Vector2.Transform(this.RadialGradientPoint, matrix);
                        float radius = Vector2.Distance(center, point);

                        ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.Array)
                        {
                            RadiusX = radius,
                            RadiusY = radius,
                            Center = center
                        };

                        ds.FillGeometry(geometry, brush);
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        Vector2 center = Vector2.Transform(this.EllipticalGradientCenter, matrix);
                        Vector2 xPoint = Vector2.Transform(this.EllipticalGradientXPoint, matrix);
                        Vector2 yPoint = Vector2.Transform(this.EllipticalGradientYPoint, matrix);

                        float radiusX = Vector2.Distance(center, xPoint);
                        float radiusY = Vector2.Distance(center, yPoint);
                        Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                            * Matrix3x2.CreateRotation(this.VectorToRadians(xPoint - center))
                            * Matrix3x2.CreateTranslation(center);

                        ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.Array)
                        {
                            Transform = transformMatrix,
                            RadiusX = radiusX,
                            RadiusY = radiusY,
                            Center = center
                        };

                        ds.FillGeometry(geometry, brush);
                    }
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Draw geometry.
        /// </summary>
        /// <param name="resourceCreator"> ICanvasResourceCreator. </param>
        /// <param name="ds"> CanvasDrawingSession. </param>
        /// <param name="geometry"> CanvasGeometry. </param>
        /// <param name="matrix"> matrix. </param>
        /// <param name="strokeWidth"> Stroke width. </param>
        public void DrawGeometry(ICanvasResourceCreator resourceCreator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix,float strokeWidth)
        {
            //Scale
            float width = strokeWidth * (matrix.M11 + matrix.M22) / 2;

            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    ds.DrawGeometry(geometry, this.Color, width);
                    break;

                case BrushType.LinearGradient:
                    {
                        Vector2 startPoint = Vector2.Transform(this.LinearGradientStartPoint, matrix);
                        Vector2 endPoint = Vector2.Transform(this.LinearGradientEndPoint, matrix);

                        ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.Array)
                        {
                            StartPoint = startPoint,
                            EndPoint = endPoint,
                        };

                        ds.DrawGeometry(geometry, brush, width);
                    }
                    break;

                case BrushType.RadialGradient:
                    {
                        Vector2 center = Vector2.Transform(this.RadialGradientCenter, matrix);
                        Vector2 point = Vector2.Transform(this.RadialGradientPoint, matrix);
                        float radius = Vector2.Distance(center, point);

                        ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.Array)
                        {
                            RadiusX = radius,
                            RadiusY = radius,
                            Center = center
                        };

                        ds.DrawGeometry(geometry, brush, width);
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        Vector2 center = Vector2.Transform(this.EllipticalGradientCenter, matrix);
                        Vector2 xPoint = Vector2.Transform(this.EllipticalGradientXPoint, matrix);
                        Vector2 yPoint = Vector2.Transform(this.EllipticalGradientYPoint, matrix);

                        float radiusX = Vector2.Distance(center, xPoint);
                        float radiusY = Vector2.Distance(center, yPoint);
                        Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                            * Matrix3x2.CreateRotation(this.VectorToRadians(xPoint - center))
                            * Matrix3x2.CreateTranslation(center);

                        ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.Array)
                        {
                            Transform = transformMatrix,
                            RadiusX = radiusX,
                            RadiusY = radiusY,
                            Center = center
                        };

                        ds.DrawGeometry(geometry, brush, width);
                    }
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }
    }
}