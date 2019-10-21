using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provides static calculations for brush positions。
    /// </summary>
    public static class BrushOperateHelper
    {

        public static BrushOperateMode ContainsNodeMode(Vector2 point, BrushType brushType, BrushPoints brushPoints, Matrix3x2 matrix)
        {
            switch (brushType)
            {
                case BrushType.None: return BrushOperateMode.LinearEndPoint;

                case BrushType.Color: return BrushOperateMode.LinearEndPoint;

                case BrushType.LinearGradient:
                    {
                        Vector2 startPoint = Vector2.Transform(brushPoints.LinearGradientStartPoint, matrix);
                        if (FanKit.Math.InNodeRadius(point, startPoint))
                        {
                            return BrushOperateMode.LinearStartPoint;
                        }

                        Vector2 endPoint = Vector2.Transform(brushPoints.LinearGradientEndPoint, matrix);
                        if (FanKit.Math.InNodeRadius(point, endPoint))
                        {
                            return BrushOperateMode.LinearEndPoint;
                        }
                    }
                    break;

                case BrushType.RadialGradient:
                    {
                        Vector2 center = Vector2.Transform(brushPoints.RadialGradientCenter, matrix);
                        if (FanKit.Math.InNodeRadius(point, center))
                        {
                            return BrushOperateMode.RadialCenter;
                        }

                        Vector2 point2 = Vector2.Transform(brushPoints.RadialGradientPoint, matrix);
                        if (FanKit.Math.InNodeRadius(point, point2))
                        {
                            return BrushOperateMode.RadialPoint;
                        }
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        Vector2 xPoint = Vector2.Transform(brushPoints.EllipticalGradientXPoint, matrix);
                        if (FanKit.Math.InNodeRadius(point, xPoint))
                        {
                            return BrushOperateMode.EllipticalXPoint;
                        }

                        Vector2 yPoint = Vector2.Transform(brushPoints.EllipticalGradientYPoint, matrix);
                        if (FanKit.Math.InNodeRadius(point, yPoint))
                        {
                            return BrushOperateMode.EllipticalYPoint;
                        }

                        Vector2 center = Vector2.Transform(brushPoints.EllipticalGradientCenter, matrix);
                        if (FanKit.Math.InNodeRadius(point, center))
                        {
                            return BrushOperateMode.EllipticalCenter;
                        }
                    }
                    break;
            }

            return BrushOperateMode.None;
        }


        public static BrushPoints? Controller(Vector2 point, BrushPoints startingBrushPoints, BrushOperateMode mode, Matrix3x2 inverseMatrix)
        {
            switch (mode)
            {
                case BrushOperateMode.LinearStartPoint:
                    {
                        Vector2 startPoint = Vector2.Transform(point, inverseMatrix);

                        startingBrushPoints.LinearGradientStartPoint = startPoint;
                        return startingBrushPoints;
                    }
                case BrushOperateMode.LinearEndPoint:
                    {
                        Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

                        startingBrushPoints.LinearGradientEndPoint = endPoint;
                        return startingBrushPoints;
                    }

                case BrushOperateMode.RadialCenter:
                    {
                        Vector2 center = Vector2.Transform(point, inverseMatrix);
                        Vector2 point2 = center + startingBrushPoints.RadialGradientPoint - startingBrushPoints.RadialGradientCenter;

                        startingBrushPoints.RadialGradientCenter = center;
                        startingBrushPoints.RadialGradientPoint = point2;
                        return startingBrushPoints;
                    }
                case BrushOperateMode.RadialPoint:
                    {
                        Vector2 point2 = Vector2.Transform(point, inverseMatrix);

                        startingBrushPoints.RadialGradientPoint = point2;
                        return startingBrushPoints;
                    }

                case BrushOperateMode.EllipticalCenter:
                    {
                        Vector2 center = Vector2.Transform(point, inverseMatrix);
                        Vector2 xPoint = center + startingBrushPoints.EllipticalGradientXPoint - startingBrushPoints.EllipticalGradientCenter;
                        Vector2 yPoint = center + startingBrushPoints.EllipticalGradientYPoint - startingBrushPoints.EllipticalGradientCenter;

                        startingBrushPoints.EllipticalGradientCenter = center;
                        startingBrushPoints.EllipticalGradientXPoint = xPoint;
                        startingBrushPoints.EllipticalGradientYPoint = yPoint;
                        return startingBrushPoints;
                    }
                case BrushOperateMode.EllipticalXPoint:
                    {
                        Vector2 xPoint = Vector2.Transform(point, inverseMatrix);

                        Vector2 normalize = Vector2.Normalize(xPoint - startingBrushPoints.EllipticalGradientCenter);
                        float radiusY = Vector2.Distance(startingBrushPoints.EllipticalGradientYPoint, startingBrushPoints.EllipticalGradientCenter);
                        Vector2 reflect = new Vector2(-normalize.Y, normalize.X);
                        Vector2 yPoint = radiusY * reflect + startingBrushPoints.EllipticalGradientCenter;

                        startingBrushPoints.EllipticalGradientXPoint = xPoint;
                        startingBrushPoints.EllipticalGradientYPoint = yPoint;
                        return startingBrushPoints;
                    }
                case BrushOperateMode.EllipticalYPoint:
                    {
                        Vector2 yPoint = Vector2.Transform(point, inverseMatrix);

                        Vector2 normalize = Vector2.Normalize(yPoint - startingBrushPoints.EllipticalGradientCenter);
                        float radiusX = Vector2.Distance(startingBrushPoints.EllipticalGradientXPoint, startingBrushPoints.EllipticalGradientCenter);
                        Vector2 reflect = new Vector2(normalize.Y, -normalize.X);
                        Vector2 xPoint = radiusX * reflect + startingBrushPoints.EllipticalGradientCenter;

                        startingBrushPoints.EllipticalGradientXPoint = xPoint;
                        startingBrushPoints.EllipticalGradientYPoint = yPoint;
                        return startingBrushPoints;
                    }
            }

            return null;
        }
        

        public static void Draw(CanvasDrawingSession drawingSession, BrushType brushType, BrushPoints brushPoints, CanvasGradientStop[] brushArray, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            switch (brushType)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        Vector2 startPoint = Vector2.Transform(brushPoints.LinearGradientStartPoint, matrix);
                        Vector2 endPoint = Vector2.Transform(brushPoints.LinearGradientEndPoint, matrix);

                        //Line: white
                        drawingSession.DrawLine(startPoint, endPoint, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(startPoint, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(endPoint, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(startPoint, endPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in brushArray)
                        {
                            Vector2 position = startPoint * (1.0f - stop.Position) + endPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        Vector2 center = Vector2.Transform(brushPoints.RadialGradientCenter, matrix);
                        Vector2 point2 = Vector2.Transform(brushPoints.RadialGradientPoint, matrix);

                        //Line: white
                        drawingSession.DrawLine(center, point2, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(point2, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, point2, accentColor, 2);

                        foreach (CanvasGradientStop stop in brushArray)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + point2 * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        Vector2 center = Vector2.Transform(brushPoints.EllipticalGradientCenter, matrix);
                        Vector2 xPoint = Vector2.Transform(brushPoints.EllipticalGradientXPoint, matrix);
                        Vector2 yPoint = Vector2.Transform(brushPoints.EllipticalGradientYPoint, matrix);

                        //Line: white
                        drawingSession.DrawLine(center, xPoint, Windows.UI.Colors.White, 4);
                        drawingSession.DrawLine(center, yPoint, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, xPoint, accentColor, 2);
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in brushArray)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }

                        //Circle: node
                        drawingSession.DrawNode2(xPoint);
                    }
                    break;
            }
        }

    }
}