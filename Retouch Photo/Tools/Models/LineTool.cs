using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models
{
    public class LineTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        Vector2 point;
        Vector2 StartPoint;

        LineLayer Layer;


        public LineTool()
        {
            base.Type = ToolType.Line;
            base.Icon = new LineControl();
            base.WorkIcon = new LineControl();
            base.Page = new LinePage();
        }

        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        public override void Start(Vector2 point)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, this.StartPoint, this.ViewModel.MarqueeMode);

            this.Layer = LineLayer.CreateFromRect(this.ViewModel.CanvasDevice, this.StartPoint, this.StartPoint, this.ViewModel.Color);
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromVector(this.StartPoint, endPoint);
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Complete(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            if (Transformer.OutNodeDistance(this.point, point))
            {
                LineLayer layer = LineLayer.CreateFromRect(this.ViewModel.CanvasDevice, this.StartPoint, endPoint, this.ViewModel.Color);
                this.ViewModel.RenderLayer.Insert(layer);
                this.ViewModel.CurrentLayer = layer;
            }

            this.Layer = null;
            this.ViewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.Layer == null) return;

            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Vector2 leftTop = Vector2.Transform(this.Layer.Transformer.DstLeftTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.Layer.Transformer.DstRightBottom, matrix);

            ds.DrawLine(leftTop, rightBottom, this.Layer.Stroke, this.Layer.StrokeWidth);
        }

    }
}
