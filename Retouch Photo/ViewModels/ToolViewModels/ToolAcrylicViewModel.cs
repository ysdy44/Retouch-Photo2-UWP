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
        VectorRect Rect => new VectorRect(this.StartPoint, this.EndPoint);

         AcrylicLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.StartPoint = this.EndPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);
            
            if (this.Layer == null) this.Layer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
            this.Layer.LayerTransformer.Rect = this.Rect;
            this.Layer.TintColor = viewModel.Color;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            this.EndPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);

            this.Layer.LayerTransformer.Rect = this.Rect;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            this.Layer.LayerTransformer.Rect = this.Rect;

            if (VectorRect.NodeDistanceOut(this.point, point))
            {
                AcrylicLayer acrylicLayer = AcrylicLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
                viewModel.RenderLayer.Insert(acrylicLayer);
            }

            viewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.Rect, Matrix3x2.CreateTranslation(this.Rect.X, this.Rect.Y) * viewModel.Transformer.CanvasToVirtualToControlMatrix);
        }

    }
}  
