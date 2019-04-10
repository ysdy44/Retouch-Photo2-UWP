using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System.Numerics;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools
{
    public abstract class IRectangleTool : ICursorTool
    {
        //@Override
        public abstract Layer GetLayer(VectRect rect);

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        Vector2 point;
        Vector2 StartPoint;

        Layer Layer;

        public override bool OperatorStart(Vector2 point)
        {
            this.point = point;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, this.StartPoint, this.ViewModel.MarqueeMode);

            this.Layer = this.GetLayer(rect);//@Override
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
            return true;
        }
        public override bool OperatorDelta(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y));
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
            return true;
        }
        public override bool OperatorComplete(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            if (Transformer.OutNodeDistance(this.point, point))
            {
                Layer Layer = this.GetLayer(rect);//@Override
                this.ViewModel.RenderLayer.Insert(Layer);
                this.ViewModel.CurrentLayer = Layer;
            }

            this.Layer = null;
            this.ViewModel.Invalidate();
            return true;
        }

        public override bool OperatorDraw(CanvasDrawingSession ds)
        {
            if (this.Layer != null)
            {
                this.Layer.Draw(this.ViewModel.CanvasDevice, ds, this.ViewModel.MatrixTransformer.Matrix);
                return true;
            }
            return false;
        }
    }
}
