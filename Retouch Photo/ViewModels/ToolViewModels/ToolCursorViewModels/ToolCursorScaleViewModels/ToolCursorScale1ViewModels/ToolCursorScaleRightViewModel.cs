using Retouch_Photo.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorScaleViewModels.ToolCursorScale1ViewModels
{
    class ToolCursorScaleRightViewModel : ToolCursorScale1ViewModel
    {
        public override Orientation GetOrientation() => Orientation.Horizontal;
        public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRight(matrix);
        public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeft(matrix);

        public override void SetPostion(Layer layer, Transformer startTransformer, float cos, float sin)
        {
            layer.Transformer.Postion.X = startTransformer.Postion.X + cos;
            layer.Transformer.Postion.Y = startTransformer.Postion.Y - sin;
        }
    }
}
