using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class Brush : ICacheTransform
    {

        /// <summary>
        /// Gets linear-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetLinearGradient(ICanvasResourceCreator resourceCreator)
        {
            Vector2 startPoint = this.Points.LinearGradientStartPoint;
            Vector2 endPoint = this.Points.LinearGradientEndPoint;

            return new CanvasLinearGradientBrush(resourceCreator, this.Array)
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
            };
        }
        /// <summary>
        /// Gets linear-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetLinearGradient(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(this.Points.LinearGradientStartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(this.Points.LinearGradientEndPoint, matrix);

            return new CanvasLinearGradientBrush(resourceCreator, this.Array)
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
            };
        }


        /// <summary>
        /// Gets radial-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetRadialGradientBrush(ICanvasResourceCreator resourceCreator)
        {
            Vector2 center = this.Points.RadialGradientCenter;
            Vector2 point = this.Points.RadialGradientPoint;

            return this._getRadialGradientBrush(resourceCreator, center, point);
        }
        /// <summary>
        /// Gets radial-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetRadialGradientBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 center = Vector2.Transform(this.Points.RadialGradientCenter, matrix);
            Vector2 point = Vector2.Transform(this.Points.RadialGradientPoint, matrix);

            return this._getRadialGradientBrush(resourceCreator, center, point);
        }
        private CanvasRadialGradientBrush _getRadialGradientBrush(ICanvasResourceCreator resourceCreator, Vector2 center, Vector2 point)
        {
            float radius = Vector2.Distance(center, point);

            return new CanvasRadialGradientBrush(resourceCreator, this.Array)
            {
                RadiusX = radius,
                RadiusY = radius,
                Center = center
            };
        }


        /// <summary>
        /// Gets elliptical-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetEllipticalGradientBrush(ICanvasResourceCreator resourceCreator)
        {
            Vector2 center = this.Points.EllipticalGradientCenter;
            Vector2 xPoint = this.Points.EllipticalGradientXPoint;
            Vector2 yPoint = this.Points.EllipticalGradientYPoint;

            return this._getEllipticalGradientBrush(resourceCreator, center, xPoint, yPoint);
        }
        /// <summary>
        /// Gets elliptical-gradient brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetEllipticalGradientBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Vector2 center = Vector2.Transform(this.Points.EllipticalGradientCenter, matrix);
            Vector2 xPoint = Vector2.Transform(this.Points.EllipticalGradientXPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.Points.EllipticalGradientYPoint, matrix);

            return this._getEllipticalGradientBrush(resourceCreator, center, xPoint, yPoint);
        }
        private CanvasRadialGradientBrush _getEllipticalGradientBrush(ICanvasResourceCreator resourceCreator, Vector2 center, Vector2 xPoint, Vector2 yPoint)
        {
            float radiusX = Vector2.Distance(center, xPoint);
            float radiusY = Vector2.Distance(center, yPoint);
            Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                * Matrix3x2.CreateRotation(FanKit.Math.VectorToRadians(xPoint - center))
                * Matrix3x2.CreateTranslation(center);

            return new CanvasRadialGradientBrush(resourceCreator, this.Array)
            {
                Transform = transformMatrix,
                RadiusX = radiusX,
                RadiusY = radiusY,
                Center = center
            };
        }


        /// <summary>
        /// Gets image brush.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The provided brush. </returns>
        public ICanvasBrush GetImageBrush(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            ImageStr imageStr = this.ImageStr;

            if (imageStr.Name==null)
            {
                return null;
            }

            ImageRe imageRe = ImageRe.FindFirstImageRe(imageStr);
            CanvasBitmap bitmap = imageRe.Source;

            Matrix3x2 matrix2 = Transformer.FindHomography(this.Source, this.ImageDestination);

            return new CanvasImageBrush(resourceCreator, bitmap)
            {
                Transform = matrix2 * matrix
            };
        }

    }
}