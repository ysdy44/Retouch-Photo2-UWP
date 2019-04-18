using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
using Retouch_Photo.Models.Layers.GeometryLayers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.Tools.Models
{
    public class PenTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public PenTool()
        {
            base.Type = ToolType.Pen;
            base.Icon = new PenControl();
            base.WorkIcon = new PenControl();
            base.Page = new PenPage();
        }


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        bool hasStartPoint;
        Vector2 startPoint;

        public override void Start(Vector2 point)
        {
            if (this.ViewModel.CurrentCurveLayer == null)
            {
                if (this.hasStartPoint==false)
                {
                    this.startPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
                    this.hasStartPoint = true;
                    this.ViewModel.Invalidate();
                    return;
                }
                else
                {
                    Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);

                    CurveLayer curveLayer = CurveLayer.CreateFromPoint(this.ViewModel.CanvasDevice, this.startPoint, endPoint, Colors.Black);
                    this.ViewModel.RenderLayer.Insert(curveLayer);
                    this.ViewModel.CurrentCurveLayer = curveLayer;

                    this.hasStartPoint = false;
                    this.ViewModel.Invalidate();
                    return;
                }
            }

            this.ViewModel.CurveNodes.Operator_Start
            (
                point,
                this.ViewModel.MatrixTransformer.Matrix,
                this.ViewModel.MatrixTransformer.InverseMatrix,
                this.ViewModel.CurrentCurveLayer.Nodes
            );
            this.ViewModel.Invalidate();
        }
        public override void Delta(Vector2 point)
        {
            if (this.ViewModel.CurrentCurveLayer == null) return;
           

            this.ViewModel.CurveNodes.Operator_Delta
   (
       point,
       this.ViewModel.MatrixTransformer.Matrix,
       this.ViewModel.MatrixTransformer.InverseMatrix,
       this.ViewModel.CurrentCurveLayer.Nodes
   );
            this.ViewModel.Invalidate();
        }
        public override void Complete(Vector2 point)
        {
            if (this.ViewModel.CurrentCurveLayer == null) return;

            this.ViewModel.CurveNodes.Operator_Complete
(
point,
this.ViewModel.MatrixTransformer.Matrix,
this.ViewModel.MatrixTransformer.InverseMatrix,
this.ViewModel.CurrentCurveLayer.Nodes
);
             this.ViewModel.Invalidate();
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.hasStartPoint)
            {
                Vector2 startPoint = Vector2.Transform(this.startPoint, this.ViewModel.MatrixTransformer.Matrix);
                Retouch_Photo.Library.HomographyController.Transformer.DrawNode(ds,startPoint);
            }

                if (this.ViewModel.CurrentCurveLayer == null) return;

            this.ViewModel.CurveNodes.Draw
(
this.ViewModel.CanvasDevice,
ds,
this.ViewModel.MatrixTransformer.Matrix,
 this.ViewModel.CurrentCurveLayer.Nodes
);
         }
    }
}
