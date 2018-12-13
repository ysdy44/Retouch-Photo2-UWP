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
        bool IsStartLimit(Vector2 point) => (this.point - point).LengthSquared() > 20.0f * 20.0f;
                     
        VectorRect Rect;
        RectangularLayer Layer;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);
            this.Rect.Start = this.Rect.End = this.StartPoint;

            if (this.Layer == null) this.Layer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
            this.Layer.Rect = this.Rect;
            this.Layer.FillBrush = new CanvasSolidColorBrush(viewModel.CanvasControl, viewModel.Color);

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {  
            this.EndPoint = Vector2.Transform(point, viewModel.Transformer.ControlToVirtualToCanvasMatrix);

            switch (viewModel.MarqueeMode)
            {
                case MarqueeMode.None:
                    this.Rect.Start = this.StartPoint;
                    this.Rect.End = this.EndPoint;
                    break;

                case MarqueeMode.Square:
                    float square = (Math.Abs(this.StartPoint.X - this.EndPoint.X) + Math.Abs(this.StartPoint.Y - this.EndPoint.Y)) / 2;
                    this.Rect.Start = this.StartPoint;
                    this.Rect.End.X = this.StartPoint.X < this.EndPoint.X ? this.StartPoint.X + square : this.StartPoint.X - square;
                    this.Rect.End.Y = this.StartPoint.Y < this.EndPoint.Y ? this.StartPoint.Y + square : this.StartPoint.Y - square;
                    break;

                case MarqueeMode.Center:
                    this.Rect.Start = this.StartPoint + this.StartPoint - this.EndPoint;
                    this.Rect.End = this.EndPoint;
                    break;

                case MarqueeMode.SquareAndCenter:
                    float square2 = (Math.Abs(this.StartPoint.X - this.EndPoint.X) + Math.Abs(this.StartPoint.Y - this.EndPoint.Y)) / 2;
                    this.Rect.Start.X = this.StartPoint.X - square2;
                    this.Rect.Start.Y = this.StartPoint.Y - square2;
                    this.Rect.End.X = this.StartPoint.X + square2;
                    this.Rect.End.Y = this.StartPoint.Y + square2;
                    break;

                default:
                        break;
                }
                        
             
            this.Layer.Rect = this.Rect;

            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            if (this.IsStartLimit(point)) viewModel.RenderLayer.Insert(RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color));
            this.Rect.Start = this.Rect.End = Vector2.Zero;

            viewModel.Invalidate(isLayerRender: true);
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            VectorRect.DrawNodeLine(ds, this.Rect, viewModel.Transformer.CanvasToVirtualToControlMatrix);
        }          

    }
}


