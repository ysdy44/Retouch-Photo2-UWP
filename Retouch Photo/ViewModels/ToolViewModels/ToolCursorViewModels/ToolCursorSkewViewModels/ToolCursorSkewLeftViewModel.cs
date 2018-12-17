using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels
{
    public class ToolCursorSkewLeftViewModel : ToolCursorSkewViewModel
    {
        public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
        public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);

        public override void SetRadian(Layer layer, Transformer startTransformer, float skew)
        {
            layer.Transformer.Skew = startTransformer.Radian + skew;
           // layer.Transformer.Radian = startTransformer.Radian + skew;
        }
    }
}
