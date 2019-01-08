using Retouch_Photo.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels.Scale1ViewModels
{
    class TopViewModel: Scale1ViewModel
    {
        public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformTop(matrix);
        public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformBottom(matrix);

        public override void SetScale(Layer layer, float scale, bool isRatio)
        {
            if (isRatio) layer.Transformer.XScale = this.StartTransformer.XScale * scale;
            layer.Transformer.YScale = this.StartTransformer.YScale * scale;
        }
        public override void SetFlip(Layer layer, bool isFlip)
        {
            layer.Transformer.FlipVertical = (this.StartTransformer.FlipVertical == isFlip);
        }
        public override void SetPostion(Layer layer, Transformer startTransformer, float xCos, float xSin, float yCos, float ySin)
        {
            if (this.StartTransformer.FlipVertical)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - ySin;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y - yCos;
            }
            else
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + ySin;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + yCos;
            }
        }
    }
}
