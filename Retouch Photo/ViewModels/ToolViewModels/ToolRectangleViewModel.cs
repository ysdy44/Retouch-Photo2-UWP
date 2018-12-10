using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
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

        Vector2 leftTopPoint;
        Vector2 rightBottomPoint;
        Vector2 rightTopPoint;
        Vector2 leftBottomPoint;
        Rect Rect => new Rect(this.leftTopPoint.ToPoint(), this.rightBottomPoint.ToPoint());

        RectangularLayer Layer; 

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            //Point
            this.point = point;
            this.leftTopPoint = this.rightBottomPoint = this.rightTopPoint = leftBottomPoint = viewModel.Transformer.InversionTransform(point);

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
            this.rightBottomPoint = viewModel.Transformer.InversionTransform(point);
            this.rightTopPoint.X = this.rightBottomPoint.X;
            this.leftBottomPoint.Y = this.rightBottomPoint.Y;

            //Layer
            this.Layer.Rect = this.Rect;

            //Invalidate
            viewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            //Point
            this.rightBottomPoint = viewModel.Transformer.InversionTransform(point);

            //Layer
            if ((this.point - point).LengthSquared() > 20.0f * 20.0f)
            {
                RectangularLayer layer = RectangularLayer.CreateFromRect(viewModel.CanvasControl, this.Rect, viewModel.Color);
                viewModel.RenderLayer.Insert(layer);
            }

            //Invalidate
            viewModel.Invalidate(isLayerRender: true);

            //Point
            this.leftTopPoint = this.rightBottomPoint = this.rightTopPoint = leftBottomPoint = Vector2.Zero;
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            Vector2 leftTop = viewModel.Transformer.Transform(this.leftTopPoint);
            Vector2 rightTop = viewModel.Transformer.Transform(this.rightTopPoint);
            Vector2 rightBottom = viewModel.Transformer.Transform(this.rightBottomPoint);
            Vector2 leftBottom = viewModel.Transformer.Transform(this.leftBottomPoint);

            ds.DrawLine(leftTop, rightTop, Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Colors.DodgerBlue);
        }

    }
}


