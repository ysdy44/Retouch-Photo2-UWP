using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolAcrylicViewModel : ToolViewModel
    {
        Vector2 point;
        Vector2 StartPoint;

         AcrylicLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            VectRect rect = new VectRect(this.StartPoint, point, viewModel.MarqueeMode);

            if (this.Layer == null) this.Layer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, rect, viewModel.Color);
            this.Layer.Transformer = Transformer.CreateFromRect(rect);
            this.Layer.TintColor = viewModel.Color;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            point = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            VectRect rect = new VectRect(this.StartPoint, point, viewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromRect(rect);

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            VectRect rect = new VectRect(this.StartPoint, point, viewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromRect(rect);

            if (Transformer.InNodeDistance(this.point, point)==false)
            {
                AcrylicLayer acrylicLayer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, rect, viewModel.Color);
                viewModel.RenderLayer.Insert(acrylicLayer);
            }

            viewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            if (this.Layer == null) return;

            Transformer.DrawBound(ds, this.Layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}  
