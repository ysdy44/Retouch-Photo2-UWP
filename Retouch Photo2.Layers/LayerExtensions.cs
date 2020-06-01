using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;

namespace Retouch_Photo2.Layers
{
    public static class LayerExtensions
    {

        public static Transform2DEffect GetHeightTransformEffect(this ICanvasImage canvasImage, ICanvasResourceCreator resourceCreator, float actualHeight)
        {
            Rect rect = canvasImage.GetBounds(resourceCreator);

            return new Transform2DEffect
            {
                TransformMatrix = rect.GetHeightMatrix(LayerageCollection.ControlsHeight),
                Source = canvasImage,
            };
        }

        public static Matrix3x2 GetHeightMatrix(this Rect rect, float actualHeight)
        {
            float width = (float)rect.Width;
            float height = (float)rect.Height;
            Vector2 center = new Vector2((float)rect.X + width / 2, (float)rect.Y + height / 2);

            float min = System.Math.Max(width, height);
            float scale = actualHeight / min;

            float halfHeight = actualHeight / 2;

            return
                Matrix3x2.CreateTranslation(-center) *
                Matrix3x2.CreateScale(scale) *
                Matrix3x2.CreateTranslation(new Vector2(halfHeight));
        }


        public static bool IsText(this LayerType  layerType)
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