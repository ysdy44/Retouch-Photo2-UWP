using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolRectangleViewModel : ToolViewModel
    {        
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;


        Vector2 point;
        Vector2 StartPoint;

        RectangularLayer Layer;

        public override void Start(Vector2 point)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            VectRect rect = new VectRect(this.StartPoint, this.StartPoint, this.ViewModel.MarqueeMode);

            if (this.Layer == null) this.Layer = RectangularLayer.CreateFromRect(this.ViewModel.CanvasControl, rect, this.ViewModel.Color);
            this.Layer.Transformer = Transformer.CreateFromRect(rect);
            this.Layer.FillBrush = new CanvasSolidColorBrush(this.ViewModel.CanvasControl, this.ViewModel.Color);

            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point)
        {  
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromRect(rect);

            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);
                        
            if (Transformer.InNodeDistance(this.point, point)==false)
            {
                RectangularLayer rectangularLayer = RectangularLayer.CreateFromRect(this.ViewModel.CanvasControl, rect, this.ViewModel.Color);
                this.ViewModel.RenderLayer.Insert(rectangularLayer);
            }

            this.ViewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.Layer == null) return;

            Transformer.DrawBound(ds, this.Layer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}


