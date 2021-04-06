using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    public partial class PenTool : ITool
    {

        Vector2 _previewLeft;
        Vector2 _previewRight;

        /// <summary> Only the left point. </summary>
        bool _hasPreviewTempLeftPoint;

        private void PreviewStart(Vector2 startingPoint)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(startingPoint, inverseMatrix);

            if (this._hasPreviewTempLeftPoint == false) this._previewLeft = canvasPoint;
            this._previewRight = canvasPoint;

            this.TipViewModel.Cursor_ManipulationStarted_Tool();
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        private void PreviewDelta(Vector2 canvasPoint)
        {
            this._previewRight = canvasPoint;

            this.ViewModel.Invalidate();//Invalidate
        }
        private void PreviewComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint, bool isOutNodeDistance)
        {
            if (this._hasPreviewTempLeftPoint)
            {
                this._hasPreviewTempLeftPoint = false;
                this.CreateLayer(this._previewLeft, canvasPoint);
            }
            else if (isOutNodeDistance)
            {
                this._hasPreviewTempLeftPoint = false;
                this.CreateLayer(canvasStartingPoint, canvasPoint);
            }
            else
            {
                this._hasPreviewTempLeftPoint = true;

                this.TipViewModel.Cursor_ManipulationStarted_None();
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            }
        }

        private void PreviewDraw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lineLeft = Vector2.Transform(this._previewLeft, matrix);

            if (this._hasPreviewTempLeftPoint)
            {
                drawingSession.DrawNode3(lineLeft);
            }
            else
            {
                Vector2 lineRight = Vector2.Transform(this._previewRight, matrix);

                drawingSession.DrawLine(lineLeft, lineRight, this.ViewModel.AccentColor);
                drawingSession.DrawNode3(lineLeft);
                drawingSession.DrawNode3(lineRight);
            }
        }


        //Add
        Node _addEndNode;
        Node _addLastNode;

        private void AddStart()
        {
            ILayer layer = this.CurveLayer;
            Layerage layerage = this.CurveLayerage;

            //Snap
            if (this.IsSnap) this.ViewModel.VectorVectorSnapInitiate(layer.Nodes);

            this._addEndNode = layer.Nodes.Last(n => n.Type == NodeType.Node);
            this._addLastNode = layer.Nodes.Last(n => n.Type == NodeType.Node);

            this.TipViewModel.Cursor_ManipulationStarted_Tool();
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        private void AddDelta(Vector2 canvasPoint)
        {
            //Snap
            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint,
                RightControlPoint = canvasPoint,
                IsChecked = false,
                IsSmooth = false,
            };
            this._addEndNode = node;

            this.ViewModel.Invalidate();//Invalidate
        }
        private void AddComplete(Vector2 canvasPoint)
        {
            ILayer layer = this.CurveLayer;
            Layerage layerage = this.CurveLayerage;

            //Snap
            if (this.IsSnap)
            {
                canvasPoint = this.Snap.Snap(canvasPoint);
                this.Snap.Default();
            }


            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_AddNode);

            //History
            var previous = layer.Nodes.NodesClone().ToList();
            history.UndoAction += () =>
            {
                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layer.Nodes.NodesReplace(previous);
            };

            //History
            this.ViewModel.HistoryPush(history);



            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint,
                RightControlPoint = canvasPoint,
                IsChecked = false,
                IsSmooth = false,
            };
            layer.Nodes.PenAdd(node);


            //Refactoring
            layer.IsRefactoringTransformer = true;
            layer.IsRefactoringRender = true;
            layer.IsRefactoringIconRender = true;
            layerage.RefactoringParentsTransformer();
            layerage.RefactoringParentsRender();
            layerage.RefactoringParentsIconRender();

            this.TipViewModel.Cursor_ManipulationStarted_None();
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        private void AddDraw(CanvasDrawingSession drawingSession)
        {
            ILayer layer = this.CurveLayer;

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lastPoint = Vector2.Transform(this._addLastNode.Point, matrix);
            Vector2 endPoint = Vector2.Transform(this._addEndNode.Point, matrix);


            //Geometry
            ICanvasBrush canvasBrush = layer.Style.Stroke.GetICanvasBrush(LayerManager.CanvasDevice);
            float strokeWidth = layer.Style.StrokeWidth;
            CanvasStrokeStyle strokeStyle = layer.Style.StrokeStyle;
            drawingSession.DrawLine(lastPoint, endPoint, canvasBrush, strokeWidth, strokeStyle);


            //Draw
            drawingSession.DrawLine(lastPoint, endPoint, this.ViewModel.AccentColor);
            drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);

            drawingSession.DrawNode3(endPoint);
            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);


            //Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}