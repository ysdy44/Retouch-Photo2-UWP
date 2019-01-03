using Retouch_Photo.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels.Scale1ViewModels
{
    class RightViewModel : Scale1ViewModel
    {
        public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRight(matrix);
        public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeft(matrix);

        public override void SetScale(Layer layer, float scale, bool isRatio)
        {
            layer.Transformer.XScale = this.StartTransformer.XScale * scale;
            if (isRatio) layer.Transformer.YScale = this.StartTransformer.YScale * scale;
        }
        public override void SetFlip(Layer layer, bool isFlip)
        {
            layer.Transformer.FlipHorizontal = (this.StartTransformer.FlipHorizontal == isFlip);
        }
        public override void SetPostion(Layer layer, Transformer startTransformer, float xCos, float xSin, float yCos, float ySin)
        {
            if (this.StartTransformer.FlipHorizontal)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + xCos;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y - xSin;
            }
            else
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - xCos;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + xSin;
            }
        }
    }
}
