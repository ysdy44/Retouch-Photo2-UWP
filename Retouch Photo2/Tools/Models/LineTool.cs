using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.Models
{
    public class LineTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;


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
        

        public override void Start(Vector2 point)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);

            this.Layer = LineLayer.CreateFromRect(this.ViewModel.CanvasDevice, this.StartPoint, this.StartPoint, this.ViewModel.Color);
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
        }
        public override void Delta(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.Layer.StartPoint = this.StartPoint;
            this.Layer.EndPoint = endPoint;
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

            this.Layer.Draw(ds, this.ViewModel.MatrixTransformer.Matrix);
        }
    }
}
