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
        Rect Rect => new Rect(this.StartPoint.ToPoint(), this.EndPoint.ToPoint());

         AcrylicLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.StartPoint = this.EndPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            
            if (this.Layer == null) this.Layer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
            this.Layer.Transformer = Transformer.CreateFromRect(this.Rect);
            this.Layer.TintColor = viewModel.Color;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            this.EndPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            this.Layer.Transformer = Transformer.CreateFromRect(this.Rect);

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            this.Layer.Transformer = Transformer.CreateFromRect(this.Rect);

            if (Transformer.NodeDistanceOut(this.point, point))
            {
                AcrylicLayer acrylicLayer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
                viewModel.RenderLayer.Insert(acrylicLayer);
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
