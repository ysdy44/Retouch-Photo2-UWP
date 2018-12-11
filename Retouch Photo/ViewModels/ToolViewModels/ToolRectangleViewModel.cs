using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
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

        VectorRect Rect;

        RectangularLayer Layer; 

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            //Point
            this.point = point;
            this.Rect.Start = this.Rect.End = viewModel.Transformer.InversionTransform(point);

            //Layer
            if (this.Layer == null) this.Layer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
            else
            {
                this.Layer.Rect = this.Rect;
                this.Layer.FillBrush = new CanvasSolidColorBrush(viewModel.CanvasControl, viewModel.Color);
            }

            //Invalidate
            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            //Point      
            this.Rect.End = viewModel.Transformer.InversionTransform(point);

            //Layer
            this.Layer.Rect = this.Rect;

            //Invalidate
            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            //Point
            this.Rect.End = viewModel.Transformer.InversionTransform(point);

            //Layer
            if ((this.point - point).LengthSquared() > 20.0f * 20.0f)
            {
                RectangularLayer layer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
                viewModel.RenderLayer.Insert(layer);
            }

            //Invalidate
            viewModel.Invalidate(isLayerRender: true);

            //Point
            this.Rect.Start = this.Rect.End = Vector2.Zero;
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.Rect, viewModel.Transformer.Matrix);
        }     


     

    }
}


