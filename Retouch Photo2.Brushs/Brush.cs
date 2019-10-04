using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class Brush : ICacheTransform
    {
        /// <summary> <see cref="Brush">'s type. </summary>
        public BrushType Type;

        /// <summary> <see cref="Brush">'s color. </summary>
        public Color Color = Colors.Gray;

        /// <summary> <see cref="Brush">'s gradient colors. </summary>
        public CanvasGradientStop[] Array = GreyWhiteMeshHelpher.GetGradientStopArray();

        /// <summary> <see cref="Brush">'s points. </summary>
        public BrushPoints Points;
        private BrushPoints _oldPoints;
        

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
                        this._oldPoints.LinearGradientStartPoint = this.Points.LinearGradientStartPoint;
                        this._oldPoints.LinearGradientEndPoint = this.Points.LinearGradientEndPoint;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this._oldPoints.RadialGradientCenter = this.Points.RadialGradientCenter;
                        this._oldPoints.RadialGradientPoint = this.Points.RadialGradientPoint;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this._oldPoints.EllipticalGradientCenter = this.Points.EllipticalGradientCenter;
                        this._oldPoints.EllipticalGradientXPoint = this.Points.EllipticalGradientXPoint;
                        this._oldPoints.EllipticalGradientYPoint = this.Points.EllipticalGradientYPoint;
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
                        this.Points.LinearGradientStartPoint = Vector2.Transform(this._oldPoints.LinearGradientStartPoint, matrix);
                        this.Points.LinearGradientEndPoint = Vector2.Transform(this._oldPoints.LinearGradientEndPoint, matrix);
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.Points.RadialGradientCenter = Vector2.Transform(this._oldPoints.RadialGradientCenter, matrix);
                        this.Points.RadialGradientPoint = Vector2.Transform(this._oldPoints.RadialGradientPoint, matrix);
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.Points.EllipticalGradientCenter = Vector2.Transform(this._oldPoints.EllipticalGradientCenter, matrix);
                        this.Points.EllipticalGradientXPoint = Vector2.Transform(this._oldPoints.EllipticalGradientXPoint, matrix);
                        this.Points.EllipticalGradientYPoint = Vector2.Transform(this._oldPoints.EllipticalGradientYPoint, matrix);
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
                        this.Points.LinearGradientStartPoint = Vector2.Add(this._oldPoints.LinearGradientStartPoint, vector);
                        this.Points.LinearGradientEndPoint = Vector2.Add(this._oldPoints.LinearGradientEndPoint, vector);
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.Points.RadialGradientCenter = Vector2.Add(this._oldPoints.RadialGradientCenter, vector);
                        this.Points.RadialGradientPoint = Vector2.Add(this._oldPoints.RadialGradientPoint, vector);
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.Points.EllipticalGradientCenter = Vector2.Add(this._oldPoints.EllipticalGradientCenter, vector);
                        this.Points.EllipticalGradientXPoint = Vector2.Add(this._oldPoints.EllipticalGradientXPoint, vector);
                        this.Points.EllipticalGradientYPoint = Vector2.Add(this._oldPoints.EllipticalGradientYPoint, vector);
                    }
                    break;
                case BrushType.Image:
                    break;
            }
        }
         
    }
}