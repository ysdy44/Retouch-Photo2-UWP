using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{
    public class ToolCursorNoneViewModel : ToolViewModel2
    {
        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }
        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }
        public override void Complete(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer, DrawViewModel viewModel)
        {
            if (viewModel.KeyCtrl) Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
            else Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }

}
