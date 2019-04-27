using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs.EllipticalGradient;
using Retouch_Photo2.Brushs.LinearGradient;
using Retouch_Photo2.Brushs.RadialGradient;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    public class Brush
    {
        public bool IsFollowTransform = true;

        public BrushType Type;

        public Color Color = Colors.Gray;
        public CanvasGradientStop[] Array = new CanvasGradientStop[]
        {
             new CanvasGradientStop{Color= Colors.White, Position=0.0f },
             new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };

        public LinearGradientManager LinearGradientManager = new LinearGradientManager();
        public RadialGradientManager RadialGradientManager = new RadialGradientManager();
        public EllipticalGradientManager EllipticalGradientManager = new EllipticalGradientManager();

        public CanvasImageBrush ImageBrush;


        public void TransformStart()
        {
            switch (this.Type)
            {  
                case BrushType.LinearGradient:
                    this.LinearGradientManager.OldStartPoint = this.LinearGradientManager.StartPoint;
                    this.LinearGradientManager.OldEndPoint = this.LinearGradientManager.EndPoint;
                    break;

                case BrushType.RadialGradient:
                    this.RadialGradientManager.OldPoint = this.RadialGradientManager.Point;
                    this.RadialGradientManager.OldCenter = this.RadialGradientManager.Center;
                    break;

                case BrushType.EllipticalGradient:
                    this.EllipticalGradientManager.OldCenter = this.EllipticalGradientManager.Center;
                    this.EllipticalGradientManager.OldXPoint = this.EllipticalGradientManager.XPoint;
                    this.EllipticalGradientManager.OldYPoint = this.EllipticalGradientManager.YPoint;
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }
        public void TransformDelta( Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.LinearGradient:
                    this.LinearGradientManager.StartPoint = Vector2.Transform(this.LinearGradientManager.OldStartPoint, matrix);
                    this.LinearGradientManager.EndPoint = Vector2.Transform(this.LinearGradientManager.OldEndPoint, matrix);
                    break;

                case BrushType.RadialGradient:
                    this.RadialGradientManager.Point = Vector2.Transform(this.RadialGradientManager.OldPoint, matrix);
                    this.RadialGradientManager.Center = Vector2.Transform(this.RadialGradientManager.OldCenter, matrix);
                    break;

                case BrushType.EllipticalGradient:
                    this.EllipticalGradientManager.Center = Vector2.Transform(this.EllipticalGradientManager.OldCenter, matrix);
                    this.EllipticalGradientManager.XPoint = Vector2.Transform(this.EllipticalGradientManager.OldXPoint, matrix);
                    this.EllipticalGradientManager.YPoint = Vector2.Transform(this.EllipticalGradientManager.OldYPoint, matrix);
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }
        public void TransformComplete (Matrix3x2 matrix)
        {
            this.TransformDelta(matrix);
        }


        public void FillGeometry(ICanvasResourceCreator creator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    ds.FillGeometry(geometry, this.Color);
                    break;

                case BrushType.LinearGradient:
                    ds.FillGeometry(geometry, this.LinearGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.RadialGradient:
                    ds.FillGeometry(geometry, this.RadialGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.EllipticalGradient:
                    ds.FillGeometry(geometry, this.EllipticalGradientManager.GetBrush(creator, matrix, this.Array));
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }


        public void DrawGeometry(ICanvasResourceCreator creator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix,float strokeWidth)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    ds.DrawGeometry(geometry, this.Color);
                    break;

                case BrushType.LinearGradient:
                    ds.DrawGeometry(geometry, this.LinearGradientManager.GetBrush(creator, matrix, this.Array), strokeWidth);
                    break;

                case BrushType.RadialGradient:
                    ds.DrawGeometry(geometry, this.RadialGradientManager.GetBrush(creator, matrix, this.Array), strokeWidth);
                    break;

                case BrushType.EllipticalGradient:
                    ds.DrawGeometry(geometry, this.EllipticalGradientManager.GetBrush(creator, matrix, this.Array),strokeWidth);
                    break;

                case BrushType.Image:
                    break;

                default:
                    break;
            }
        }
    }
}
