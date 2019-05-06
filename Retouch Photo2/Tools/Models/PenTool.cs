using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Library;
using Retouch_Photo2.Models.Layers.GeometryLayers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Tools.Models
{
    public class PenTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        public PenTool()
        {
            base.Type = ToolType.Pen;
            base.Icon = new PenControl();
            base.WorkIcon = new PenControl();
            base.Page = new PenPage();
        }

        
        bool hasStartPoint;
        Vector2 startPoint;

        public override void Start(Vector2 point)
        {
            if (this.ViewModel.CurveLayer == null)
            {
                if (this.hasStartPoint == false)
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
                    this.ViewModel.CurveLayer = curveLayer;

                    this.hasStartPoint = false;
                    this.ViewModel.Invalidate();
                    return;
                }
            }

            this.ViewModel.CurveNodes.Operator_Start(point, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix, this.ViewModel.CurveLayer.Nodes);
            this.ViewModel.Invalidate();
        }
        public override void Delta(Vector2 point)
        {
            if (this.ViewModel.CurveLayer == null) return;

            this.ViewModel.CurveNodes.Operator_Delta(point, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix, this.ViewModel.CurveLayer.Nodes);

            //ResetTransformer
            this.ViewModel.CurveLayer.ResetNodesGeometryByNodes();
            this.ViewModel.Invalidate();
        }
        public override void Complete(Vector2 point)
        {
            if (this.ViewModel.CurveLayer == null) return;

            this.ViewModel.CurveNodes.Operator_Complete(point, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix, this.ViewModel.CurveLayer.Nodes);

            //ResetTransformer
            this.ViewModel.CurveLayer.ResetNodesGeometryByNodes();
            this.ViewModel.CurveLayer.ResetTransformerByNodesGeometry();
            this.ViewModel.Invalidate();
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            if (this.hasStartPoint)
            {
                Vector2 startPoint = Vector2.Transform(this.startPoint, this.ViewModel.MatrixTransformer.Matrix);
                HomographyController.Transformer.DrawNode(ds, startPoint);
            }

            if (this.ViewModel.CurveLayer == null) return;

            //Geometry 
            CanvasGeometry geometry = this.ViewModel.CurveLayer.NodesGeometry.Transform(this.ViewModel.MatrixTransformer.Matrix);
            ds.DrawGeometry(geometry, Colors.DodgerBlue, 2);

            this.ViewModel.CurveNodes.Draw
            (
                this.ViewModel.CanvasDevice, 
                ds,
                this.ViewModel.MatrixTransformer.Matrix,
                this.ViewModel.CurveLayer.Nodes
            );
        }
    }
}
