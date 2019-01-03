using Retouch_Photo.Models;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.SkewViewModels
{
    public class BottomViewModel : SkewViewModel
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
