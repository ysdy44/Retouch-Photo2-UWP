using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{
    public class ToolCursorTranslationViewModel : ToolViewModel2
    {
        Vector2 StartTransformerPostion;
        Vector2 StartPostion;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            this.StartPostion = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            this.StartTransformerPostion = layer.Transformer.Postion;
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            layer.Transformer.Postion =this.StartTransformerPostion -this. StartPostion + Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            Transformer.DrawBound(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}
