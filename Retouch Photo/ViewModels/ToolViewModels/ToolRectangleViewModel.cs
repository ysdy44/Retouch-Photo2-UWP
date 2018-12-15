using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.Library;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    public class ToolRectangleViewModel : ToolViewModel
    {
        Vector2 point;
        Vector2 StartPoint;
        Vector2 EndPoint;
        Rect GetRect() => new Rect(this.StartPoint.ToPoint(),this.EndPoint.ToPoint());

        RectangularLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.StartPoint = this.EndPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
                  
            if (this.Layer == null) this.Layer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.GetRect(), viewModel.Color);
            this.Layer.Transformer = Transformer.CreateFromRect(this.GetRect());
            this.Layer.FillBrush = new CanvasSolidColorBrush(viewModel.CanvasControl, viewModel.Color);

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {  
            this.EndPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);

            this.Layer.Transformer = Transformer.CreateFromRect(this.GetRect());

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            this.Layer.Transformer = Transformer.CreateFromRect(this.GetRect());
            
            if (Transformer.NodeDistanceOut(this.point, point))
            {
                RectangularLayer rectangularLayer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.GetRect(), viewModel.Color);
                viewModel.RenderLayer.Insert(rectangularLayer);
            }

            viewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            if (this.Layer == null) return;

            Transformer.DrawNodeLine(ds, this.Layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
        }

    }
}


