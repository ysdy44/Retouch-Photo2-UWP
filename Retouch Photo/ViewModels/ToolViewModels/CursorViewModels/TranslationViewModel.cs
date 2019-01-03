using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels
{
    public class TranslationViewModel : ToolViewModel2
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        Vector2 StartTransformerPostion;
        Vector2 StartPostion;

        public override void Start(Vector2 point, Layer layer)
        {
            this.StartPostion = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            this.StartTransformerPostion = layer.Transformer.Postion;
        }
        public override void Delta(Vector2 point, Layer layer)
        {
            layer.Transformer.Postion =this.StartTransformerPostion -this. StartPostion + Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
        }
        public override void Complete(Vector2 point, Layer layer)
        {
        }

        public override void Draw(CanvasDrawingSession ds, Layer layer)
        {
            Transformer.DrawBound(ds, layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}
