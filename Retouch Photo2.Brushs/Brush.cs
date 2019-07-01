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

        /// <summary> <see cref="Brush">'s LinearGradientManager. </summary>
        public LinearGradientManager LinearGradientManager = new LinearGradientManager();
        /// <summary> <see cref="Brush">'s RadialGradientManager. </summary>
        public RadialGradientManager RadialGradientManager = new RadialGradientManager();
        /// <summary> <see cref="Brush">'s EllipticalGradientManager. </summary>
        public EllipticalGradientManager EllipticalGradientManager = new EllipticalGradientManager();
        /// <summary> <see cref="Brush">'s CanvasImageBrush. </summary>
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
        public void TransformDelta(Matrix3x2 matrix)
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
        public void TransformComplete(Matrix3x2 matrix)
        {
            this.TransformDelta(matrix);
        }


        /// <summary>
        /// Fill geometry.
        /// </summary>
        /// <param name="creator"> ICanvasResourceCreator. </param>
        /// <param name="ds"> CanvasDrawingSession. </param>
        /// <param name="geometry"> CanvasGeometry. </param>
        /// <param name="matrix"> matrix. </param>
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
                    {
                        ICanvasBrush brush = this.LinearGradientManager.GetBrush(creator, matrix, this.Array);
                        ds.FillGeometry(geometry, brush);
                    }
                    break;

                case BrushType.RadialGradient:
                    {
                        ICanvasBrush brush = this.RadialGradientManager.GetBrush(creator, matrix, this.Array);
                        ds.FillGeometry(geometry, brush);
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        ICanvasBrush brush = this.EllipticalGradientManager.GetBrush(creator, matrix, this.Array);
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
        /// <param name="creator"> ICanvasResourceCreator. </param>
        /// <param name="ds"> CanvasDrawingSession. </param>
        /// <param name="geometry"> CanvasGeometry. </param>
        /// <param name="matrix"> matrix. </param>
        /// <param name="strokeWidth"> Stroke width. </param>
        public void DrawGeometry(ICanvasResourceCreator creator, CanvasDrawingSession ds, CanvasGeometry geometry, Matrix3x2 matrix,float strokeWidth)
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
                        ICanvasBrush brush = this.LinearGradientManager.GetBrush(creator, matrix, this.Array);
                        ds.DrawGeometry(geometry, brush, width);
                    }
                    break;

                case BrushType.RadialGradient:
                    {
                        ICanvasBrush brush = this.RadialGradientManager.GetBrush(creator, matrix, this.Array);
                        ds.DrawGeometry(geometry, brush, width);
                    }
                    break;

                case BrushType.EllipticalGradient:
                    {
                        ICanvasBrush brush = this.EllipticalGradientManager.GetBrush(creator, matrix, this.Array);
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