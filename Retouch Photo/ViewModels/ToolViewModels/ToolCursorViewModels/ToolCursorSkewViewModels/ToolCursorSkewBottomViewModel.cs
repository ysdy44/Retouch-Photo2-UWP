using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels
{
    public class ToolCursorSkewBottomViewModel : ToolCursorSkewViewModel
    {
        public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
        public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);

        public override void SetRadian(Layer layer, Transformer startTransformer, float skew)
        {
            float value = -skew + startTransformer.Radian + Transformer.PiHalf;
            layer.Transformer.Skew = value;
        }
    }
}
