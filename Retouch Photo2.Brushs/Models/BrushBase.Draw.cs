using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    public partial class BrushBase : IBrush
    {

        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Color accentColor)
        {
            switch (this.Type)
            {
                case BrushType.None: break;
                case BrushType.Color:
                    break;
            }


            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);

            switch (this.Type)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, yPoint, Windows.UI.Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Windows.UI.Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Windows.UI.Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in this.Stops)
                        {
                            Vector2 position = center * (1.0f - stop.Position) + yPoint * stop.Position;

                            //Circle: stop
                            drawingSession.FillCircle(position, 8, accentColor);
                            drawingSession.FillCircle(position, 6, stop.Color);
                        }
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, xPoint, Colors.White, 4);
                        drawingSession.DrawLine(center, yPoint, Colors.White, 4);

                        //Circle: white
                        drawingSession.FillCircle(center, 10, Colors.White);
                        drawingSession.FillCircle(yPoint, 10, Colors.White);

                        //Line: accent
                        drawingSession.DrawLine(center, xPoint, accentColor, 2);
                        drawingSession.DrawLine(center, yPoint, accentColor, 2);

                        foreach (CanvasGradientStop stop in this.Stops)
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
                case BrushType.Image:
                    {
                        //Line: white
                        drawingSession.DrawLine(center, xPoint, Colors.White, 4);
                        drawingSession.DrawLine(center, yPoint, Colors.White, 4);

                        //Line: accent
                        drawingSession.DrawLine(center, xPoint, Colors.LimeGreen, 2); 
                        drawingSession.DrawLine(center, yPoint, Colors.Red, 2);

                        //Circle: node
                        drawingSession.DrawNode2(center);
                        drawingSession.DrawNode2(xPoint);
                        drawingSession.DrawNode2(yPoint);
                    }
                    break;
                default:
                    break;
            }
        }

    }
}