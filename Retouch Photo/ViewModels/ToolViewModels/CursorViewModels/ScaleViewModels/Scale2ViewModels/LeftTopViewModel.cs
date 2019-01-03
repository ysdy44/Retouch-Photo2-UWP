using Retouch_Photo.Models;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels.Scale2ViewModels
{
    public class LeftTopViewModel : Scale2ViewModel
    {
        public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
        public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
        public override Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
        public override Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);

        public override void SetPostion(Layer layer, Transformer startTransformer, float xCos, float xSin, float yCos, float ySin)
        {
            if (startTransformer.Skew == 0)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - xCos - ySin;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + xSin - yCos;
            }
            else
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - xCos + ySin;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + xSin + yCos;
            }
        }
    }
}
