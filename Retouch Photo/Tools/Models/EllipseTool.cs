using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models
{
    public class EllipseTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;


        Vector2 point;
        Vector2 StartPoint;

        EllipseLayer EllipseLayer;


        public EllipseTool()
        {
            base.Type = ToolType.Ellipse;
            base.Icon = new EllipseControl();
            base.WorkIcon = new EllipseControl();
            base.Page = new EllipsePage();
        }

        public bool IsTransformer(TransformerMode mode)
        {
            if (mode == TransformerMode.None) return false;
            if (mode == TransformerMode.Translation) return false;
            return true;
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
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;


            // Transformer
            if (this.ViewModel.TransformerStart(point)) return;


            this.point = point;
            this.StartPoint = Vector2.Transform(point, inverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, this.StartPoint, this.ViewModel.MarqueeMode);

            this.EllipseLayer = EllipseLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.EllipseLayer);
        }
        public override void Delta(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;


            // Transformer
            if (this.ViewModel.TransformerDelta(point)) return;


            Vector2 endPoint = Vector2.Transform(point, inverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.EllipseLayer.Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y));
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.EllipseLayer);
        }
        public override void Complete(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;


            // Transformer
           if (this.ViewModel.TransformerComplete(point)) return;


            Vector2 endPoint = Vector2.Transform(point, inverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            if (Transformer.OutNodeDistance(this.point, point))
            {
                EllipseLayer ellipseLayer = EllipseLayer.CreateFromRect(this.ViewModel.CanvasDevice, rect, this.ViewModel.Color);
                this.ViewModel.RenderLayer.Insert(ellipseLayer);
                this.ViewModel.CurrentLayer = ellipseLayer;
            }

            this.EllipseLayer = null;
            this.ViewModel.Invalidate();
        }


        public override void Draw(CanvasDrawingSession ds)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;

            if (this.EllipseLayer != null)
            {
                this.EllipseLayer.Draw(this.ViewModel.CanvasDevice, ds, matrix);
                return;
            }

            //Transformer      
            if (this.ViewModel.TransformerDraw(ds)) return;
        }
    }
}
