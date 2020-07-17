using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Extensions of <see cref="ILayer"/>.
    /// </summary>
    public static class LayerExtensions
    {

        /// <summary>
        /// Draw a layer's lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="layer"> The layer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawLayerBound(this CanvasDrawingSession drawingSession,ILayer layer, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            layer.DrawBound(drawingSession, matrix, accentColor);
        }


        /// <summary>
        /// Turn into icon render image.
        /// </summary>
        /// <param name="canvasImage"> The canvas image. </param>
        /// <param name="resourceCreator"> The resource creator. </param>
        /// <param name="actualHeight"> The actual height. </param>
        /// <returns> The product image. </returns>
        public static ICanvasImage ToIconRenderImage(this ICanvasImage canvasImage, ICanvasResourceCreator resourceCreator, float actualHeight)
        {
            Rect bound = canvasImage.GetBounds(resourceCreator);

            return new Transform2DEffect
            {
                TransformMatrix = bound.ToIconRenderMatrix(LayerageCollection.ControlsHeight),
                Source = canvasImage,
            };
        }

        /// <summary>
        /// Turn into icon render matrix.
        /// </summary>
        /// <param name="bound"> The icon bound. </param>
        /// <param name="actualHeight"> The actual height. </param>
        /// <returns> The product matrix. </returns>
        public static Matrix3x2 ToIconRenderMatrix(this Rect bound, float actualHeight)
        {
            float width = (float)bound.Width;
            float height = (float)bound.Height;
            Vector2 center = new Vector2((float)bound.X + width / 2, (float)bound.Y + height / 2);

            float min = System.Math.Max(width, height);
            float scale = actualHeight / min;

            float halfHeight = actualHeight / 2;

            return
                Matrix3x2.CreateTranslation(-center) *
                Matrix3x2.CreateScale(scale) *
                Matrix3x2.CreateTranslation(new Vector2(halfHeight));
        }


        /// <summary>
        /// Is text type?
        /// </summary>
        /// <param name="layerType"> The layer type. </param>
        public static bool IsText(this LayerType layerType)
        {
            switch (layerType)
            {
                case LayerType.TextArtistic:
                case LayerType.TextFrame:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Is scale mode?
        /// </summary>
        /// <param name="transformerMode"> The transformer mode. </param>
        public static bool IsScale(this TransformerMode transformerMode)
        {
            switch (transformerMode)
            {
                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    return true;
                default:
                    return false;
            }
        }

    }
}