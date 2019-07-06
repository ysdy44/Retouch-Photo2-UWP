using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Brush Classes.
    /// </summary>
    public class Brush: ICacheTransform
    {
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
        
        /// <summary> <see cref="Brush">'s points. </summary>
        public BrushPoints Points;
        /// <summary> <see cref = "Brush.Points" />'s old cache. </summary>
        public BrushPoints OldPoints;
        

        //@Interface
        /// <summary>
        ///  Cache the brush's transformer.
        /// </summary>
        public void CacheTransform()
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.OldPoints.LinearGradientStartPoint = this.Points.LinearGradientStartPoint;
                        this.OldPoints.LinearGradientEndPoint = this.Points.LinearGradientEndPoint;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.OldPoints.RadialGradientCenter = this.Points.RadialGradientCenter;
                        this.OldPoints.RadialGradientPoint = this.Points.RadialGradientPoint;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.OldPoints.EllipticalGradientCenter = this.Points.EllipticalGradientCenter;
                        this.OldPoints.EllipticalGradientXPoint = this.Points.EllipticalGradientXPoint;
                        this.OldPoints.EllipticalGradientYPoint = this.Points.EllipticalGradientYPoint;
                    }
                    break;
                case BrushType.Image:
                    break;
            }
        }
        /// <summary>
        ///  Transforms the brush by the given matrix.
        /// </summary>
        /// <param name="matrix"> The sestination matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.Points.LinearGradientStartPoint = Vector2.Transform(this.OldPoints.LinearGradientStartPoint, matrix);
                        this.Points.LinearGradientEndPoint = Vector2.Transform(this.OldPoints.LinearGradientEndPoint, matrix);
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.Points.RadialGradientCenter = Vector2.Transform(this.OldPoints.RadialGradientCenter, matrix);
                        this.Points.RadialGradientPoint = Vector2.Transform(this.OldPoints.RadialGradientPoint, matrix);
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.Points.EllipticalGradientCenter = Vector2.Transform(this.OldPoints.EllipticalGradientCenter, matrix);
                        this.Points.EllipticalGradientXPoint = Vector2.Transform(this.OldPoints.EllipticalGradientXPoint, matrix);
                        this.Points.EllipticalGradientYPoint = Vector2.Transform(this.OldPoints.EllipticalGradientYPoint, matrix);
                    }
                    break;
                case BrushType.Image:
                    break;
            }
        }
        /// <summary>
        ///  Transforms the brush by the given vector.
        /// </summary>
        /// <param name="vector"> The sestination vector. </param>
        public void TransformAdd(Vector2 vector)
        {
            switch (this.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.Points.LinearGradientStartPoint = Vector2.Add(this.OldPoints.LinearGradientStartPoint, vector);
                        this.Points.LinearGradientEndPoint = Vector2.Add(this.OldPoints.LinearGradientEndPoint, vector);
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.Points.RadialGradientCenter = Vector2.Add(this.OldPoints.RadialGradientCenter, vector);
                        this.Points.RadialGradientPoint = Vector2.Add(this.OldPoints.RadialGradientPoint, vector);
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.Points.EllipticalGradientCenter = Vector2.Add(this.OldPoints.EllipticalGradientCenter, vector);
                        this.Points.EllipticalGradientXPoint = Vector2.Add(this.OldPoints.EllipticalGradientXPoint, vector);
                        this.Points.EllipticalGradientYPoint = Vector2.Add(this.OldPoints.EllipticalGradientYPoint, vector);
                    }
                    break;
                case BrushType.Image:
                    break;
            }
        }


        //@Static
        /// <summary> 
        /// Gets new CanvasGradientStop array. 
        /// </summary>
        /// <returns> stops </returns>
        public static CanvasGradientStop[] GetNewArray() => new CanvasGradientStop[]
        {
            new CanvasGradientStop{Color= Colors.White, Position=0.0f },
            new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
        };
        /// <summary>
        /// Create a gray-and-white bitmap.
        /// </summary>
        /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="width"> The bitmap width. </param>
        /// <param name="height"> The bitmap height. </param>
        /// <param name="columns"> Number of columns. </param>
        /// <returns> CanvasRenderTarget </returns>
        public static CanvasRenderTarget CreateGrayAndWhiteBackground(ICanvasResourceCreatorWithDpi resourceCreator, float width, float height,int columns=4)
        {
            CanvasRenderTarget background = new CanvasRenderTarget(resourceCreator, width, height);
            
            Color[] colors = new Color[]
            {
                  Windows.UI.Colors.LightGray,
                  Windows.UI.Colors.White,
                  Windows.UI.Colors.White,
                  Windows.UI.Colors.LightGray
            };

            CanvasBitmap bitmap = CanvasBitmap.CreateFromColors(resourceCreator, colors, 2, 2);

            using (CanvasDrawingSession ds = background.CreateDrawingSession())
            {
                ds.DrawImage(new DpiCompensationEffect
                {
                    Source = new ScaleEffect
                    {
                        Scale = new Vector2(height / columns),
                        InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                        Source = new BorderEffect
                        {
                            ExtendX = CanvasEdgeBehavior.Wrap,
                            ExtendY = CanvasEdgeBehavior.Wrap,
                            Source = bitmap
                        }
                    }
                });
            }

            return background;
        }

    }
}