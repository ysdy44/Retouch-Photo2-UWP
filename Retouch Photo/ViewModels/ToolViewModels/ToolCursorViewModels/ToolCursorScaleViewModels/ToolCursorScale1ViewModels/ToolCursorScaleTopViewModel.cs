using Retouch_Photo.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorScaleViewModels.ToolCursorScale1ViewModels
{
    class ToolCursorScaleTopViewModel: ToolCursorScale1ViewModel
    {
        public override Orientation GetOrientation() => Orientation.Vertical;
        public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformTop(matrix);
        public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformBottom(matrix);

        public override void SetPostion(Layer layer, Transformer startTransformer, float cos, float sin)
        {
            layer.Transformer.Postion.X = startTransformer.Postion.X - sin;
            layer.Transformer.Postion.Y = startTransformer.Postion.Y - cos;
        }
    }
}
