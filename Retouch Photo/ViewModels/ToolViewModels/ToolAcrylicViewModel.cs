using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo.Library;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
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
    public class ToolAcrylicViewModel : ToolViewModel
    {
        Vector2 point;
        Vector2 StartPoint;
        Vector2 EndPoint;
        bool IsStartLimit(Vector2 point) => (this.point - point).LengthSquared() > 20.0f * 20.0f;

        VectorRect Rect;
        AcrylicLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.Rect.Start = this.Rect.End = this.StartPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);
 
            if (this.Layer == null) this.Layer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl,this.Rect, viewModel.Color);
            this.Layer.Rect = this.Rect;
            this.Layer.TintColor = viewModel.Color;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            this.EndPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);

            this.Layer.Rect = this.Rect = new VectorRect(this.StartPoint, this.EndPoint, viewModel.MarqueeMode);

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            this.Rect = new VectorRect(this.StartPoint, this.EndPoint, viewModel.MarqueeMode);

            if (this.IsStartLimit(point)) viewModel.RenderLayer.Insert(AcrylicLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color));
            this.Rect.Start = this.Rect.End = Vector2.Zero;

            viewModel.Invalidate(isLayerRender: true);
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.Rect, viewModel.Transformer.CanvasToVirtualToControlMatrix);
        }

    }
}  
