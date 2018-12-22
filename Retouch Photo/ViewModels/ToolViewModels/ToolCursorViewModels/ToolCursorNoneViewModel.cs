using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels
{
    public class ToolCursorNoneViewModel : ToolViewModel2
    {    
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsSkew => this.ViewModel.KeyAlt;


        public override void Start(Vector2 point, Layer layer)
        {
        }
        public override void Delta(Vector2 point, Layer layer)
        {
        }
        public override void Complete(Vector2 point, Layer layer)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer)
        {
            if (IsSkew)
                Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
            else
                Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }

}
