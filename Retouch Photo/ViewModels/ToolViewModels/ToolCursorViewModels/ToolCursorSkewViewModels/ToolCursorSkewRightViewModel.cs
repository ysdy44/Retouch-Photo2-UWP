using Retouch_Photo.Models;
using System;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels
{
    public class ToolCursorSkewRightViewModel : ToolCursorSkewViewModel
    {
        public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
        public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);

        public override void SetRadian(Layer layer, Transformer startTransformer, float skew)
        {
            float value = skew + startTransformer.Skew - startTransformer.Radian;
            layer.Transformer.Skew = value;

            layer.Transformer.Radian = skew;
            float cos = (float)Math.Abs(Math.Cos(value));
            layer.Transformer.XScale = startTransformer.XScale / cos;
            layer.Transformer.YScale = startTransformer.YScale * cos;
        }
    }
}

