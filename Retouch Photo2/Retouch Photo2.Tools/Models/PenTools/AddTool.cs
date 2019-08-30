using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenAddTool.
    /// </summary>
    public class AddTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        NodeCollection NodeCollection => this.SelectionViewModel.CurveLayer.NodeCollection;

        Node _endNode;
        Node _lastNode;

        public void Start(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint,
                RightControlPoint = canvasPoint,
                IsChecked = false,
                IsSmooth = false,
            };
            this._endNode = node;
            this._lastNode = this.SelectionViewModel.CurveLayer.NodeCollection.Last();
            this.ViewModel.Invalidate();
        }
        public void Delta(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint,
                RightControlPoint = canvasPoint,
                IsChecked = false,
                IsSmooth = false,
            };
            this._endNode = node;
            this.ViewModel.Invalidate();
        }
        public void Complete(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint,
                RightControlPoint = canvasPoint,
                IsChecked = false,
                IsSmooth = false,
            };
            this.NodeCollection.Add(node);
            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
            this.ViewModel.Invalidate();//Invalidate
        }

        /// <summary>
        /// Draw geometry stroke.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lastPoint = Vector2.Transform(this._lastNode.Point, matrix);
            Vector2 endPoint = Vector2.Transform(this._endNode.Point, matrix);

            drawingSession.DrawLineDodgerBlue(lastPoint, endPoint);
            drawingSession.DrawNode4(endPoint);
        }
    }
}