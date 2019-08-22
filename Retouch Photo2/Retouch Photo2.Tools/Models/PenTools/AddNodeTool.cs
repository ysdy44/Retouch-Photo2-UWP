using FanKit.Transformers;
using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenAddNodeTool.
    /// </summary>
    public class AddNodeTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        Node _endNode;
        Node _lastNode;

        public void Start(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this._endNode = new Node(canvasPoint);
            this._lastNode = this.SelectionViewModel.CurveLayer.Nodes.Last();
            this.ViewModel.Invalidate();
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this._endNode.Vector = canvasPoint;
            this.ViewModel.Invalidate();
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Node node = new Node(canvasPoint);
            this.Nodes.Add(node);
            this.ViewModel.Invalidate();
        }

        /// <summary>
        /// Draw geometry stroke.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lastPoint = Vector2.Transform(this._lastNode.Vector, matrix);
            Vector2 endPoint = Vector2.Transform(this._endNode.Vector, matrix);
            
            drawingSession.DrawLineDodgerBlue(lastPoint, endPoint);
            drawingSession.DrawNode4(endPoint);
        }
    }
}