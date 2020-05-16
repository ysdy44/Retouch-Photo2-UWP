using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushHandle : ICacheTransform
    {

        public CanvasLinearGradientBrush LinearGradientGetICanvasBrush(ICanvasResourceCreator resourceCreator, CanvasGradientStop[] stops, Vector2 center, Vector2 xPoint)
        {
            return new CanvasLinearGradientBrush(resourceCreator, stops)
            {
                StartPoint = center,
                EndPoint = xPoint,
            };
        }

        private CanvasRadialGradientBrush GetRadialGradientBrush(ICanvasResourceCreator resourceCreator, CanvasGradientStop[] stops, Vector2 center, Vector2 point)
        {
            float radius = Vector2.Distance(center, point);

            return new CanvasRadialGradientBrush(resourceCreator, stops)
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = center
            };
        }

        public CanvasRadialGradientBrush GetEllipticalGradientBrush(ICanvasResourceCreator resourceCreator, CanvasGradientStop[] stops, Vector2 center, Vector2 xPoint, Vector2 yPoint)
        {
            float radiusX = Vector2.Distance(center, xPoint);
            float radiusY = Vector2.Distance(center, yPoint);
            Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                * Matrix3x2.CreateRotation(FanKit.Math.VectorToRadians(xPoint - center))
                * Matrix3x2.CreateTranslation(center);

            return new CanvasRadialGradientBrush(resourceCreator, stops)
            {
                Transform = transformMatrix,
                RadiusX = radiusX,
                RadiusY = radiusY,
                Center = center
            };
        }





        public BrushOperateMode ContainsOperateMode(Vector2 point, Matrix3x2 matrix, bool hasYPoint = false)
        {
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            if (FanKit.Math.InNodeRadius(point, xPoint))
            {
                return BrushOperateMode.XPoint;
            }

            if (hasYPoint)
            {
                Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
                if (FanKit.Math.InNodeRadius(point, yPoint))
                {
                    return BrushOperateMode.YPoint;
                }
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (FanKit.Math.InNodeRadius(point, center))
            {
                return BrushOperateMode.Center;
            }

            return BrushOperateMode.None;
        }




        public void EllipticalController(BrushOperateMode mode, Vector2 startingPoint, Vector2 point)
        {
            switch (mode)
            {
                case BrushOperateMode.Center:
                    Vector2 offset = point - startingPoint;
                    this.Center = point;
                    this.XPoint = offset + this._startingXPoint;
                    this.YPoint = offset + this._startingYPoint;
                    break;
                case BrushOperateMode.XPoint:
                    {
                        Vector2 xPoint = point;

                        Vector2 normalize = Vector2.Normalize(xPoint - this._startingCenter);
                        float radiusY = Vector2.Distance(this._startingYPoint, this._startingCenter);
                        Vector2 reflect = new Vector2(-normalize.Y, normalize.X);
                        Vector2 yPoint = radiusY * reflect + this._startingCenter;

                        this.XPoint = xPoint;
                        this.YPoint = yPoint;
                    }
                    break;
                case BrushOperateMode.YPoint:
                    {
                        Vector2 yPoint = point;

                        Vector2 normalize = Vector2.Normalize(yPoint - this._startingCenter);
                        float radiusX = Vector2.Distance(this._startingXPoint, this._startingCenter);
                        Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
                        Vector2 xPoint = radiusX * reflect + this._startingCenter;

                        this.XPoint = xPoint;
                        this.YPoint = yPoint;
                    }
                    break;
            }
        }



        public void LinearGradientDraw(CanvasDrawingSession drawingSession,CanvasGradientStop[] stops, Matrix3x2 matrix, Color accentColor)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);

            //Line: white
            drawingSession.DrawLine(center, xPoint, Windows.UI.Colors.White, 4);

            //Circle: white
            drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
            drawingSession.FillCircle(xPoint, 10, Windows.UI.Colors.White);

            //Line: accent
            drawingSession.DrawLine(center, xPoint, accentColor, 2);

            foreach (CanvasGradientStop stop in stops)
            {
                Vector2 position = center * (1.0f - stop.Position) + xPoint * stop.Position;

                //Circle: stop
                drawingSession.FillCircle(position, 8, accentColor);
                drawingSession.FillCircle(position, 6, stop.Color);
            }
        }


        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            //Line: white
            drawingSession.DrawLine(center, xPoint, Colors.White, 4);
            drawingSession.DrawLine(center, yPoint, Colors.White, 4);

            //Line: accent
            drawingSession.DrawLine(center, xPoint, Colors.Red, 2);
            drawingSession.DrawLine(center, yPoint, Colors.LimeGreen, 2);

            //Circle: node
            drawingSession.DrawNode2(center);
            drawingSession.DrawNode2(xPoint);
            drawingSession.DrawNode2(yPoint);
        }



        public void EllipticalGradientDraw(CanvasDrawingSession drawingSession,CanvasGradientStop[] stops, Matrix3x2 matrix, Color accentColor)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            //Line: white
            drawingSession.DrawLine(center, xPoint, Colors.White, 4);
            drawingSession.DrawLine(center, yPoint, Colors.White, 4);

            //Circle: white
            drawingSession.FillCircle(center, 10, Colors.White);
            drawingSession.FillCircle(yPoint, 10, Colors.White);

            //Line: accent
            drawingSession.DrawLine(center, xPoint, accentColor, 2);
            drawingSession.DrawLine(center, yPoint, accentColor, 2);

            foreach (CanvasGradientStop stop in stops)
            {
                Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                //Circle: stop
                drawingSession.FillCircle(position, 8, accentColor);
                drawingSession.FillCircle(position, 6, stop.Color);
            }

            //Circle: node
            drawingSession.DrawNode2(xPoint);
        }





    }
}