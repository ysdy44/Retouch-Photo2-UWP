using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorSkewViewModels
{
    public class ToolCursorSkewBottomViewModel : ToolCursorSkewViewModel
    {
        public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
        public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
        public override float GetStartRadian(Layer layer) => layer.Transformer.RadianX;
        public override void SetRadian(Layer layer, float value) => layer.Transformer.RadianX = value;
    }
}
