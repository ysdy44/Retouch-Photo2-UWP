using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels
{
    public class NoneViewModel : IToolViewModel
    {    
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsSkew => this.ViewModel.KeyAlt;

        public void Start(Vector2 point, Layer layer) { }
        public void Delta(Vector2 point, Layer layer) { }
        public void Complete(Vector2 point, Layer layer) { }

        public void Draw(CanvasDrawingSession ds, Layer layer)
        {
            if (IsSkew)
                Transformer.DrawBoundNodesWithSkew(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
            else
                Transformer.DrawBoundNodesWithRotation(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }
    }
}
