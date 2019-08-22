using FanKit.Transformers;
using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenRectChoose.
    /// </summary>
    public class RectChooseTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        public TransformerRect Rect;
        private bool _isPressed = false;

        public void Start(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this.Rect = new TransformerRect(canvasPoint, canvasPoint);
            this._isPressed = true;
            
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint=Vector2.Transform(point, inverseMatrix);

            this.Rect = new TransformerRect(canvasStartingPoint, canvasPoint);

            //Choose point which in the rect.
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                bool isContained = this.Nodes[i].Contained(this.Rect);
                this.Nodes[i].IsChecked = isContained;
            }
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            this._isPressed = false;
            this.Delta(startingPoint,point);            
        }

        /// <summary>
        /// Draw a choose rectangle.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this._isPressed == false) return;

            //LTRB
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 leftTop = Vector2.Transform(this.Rect.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.Rect.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.Rect.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.Rect.LeftBottom, matrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            };

            //Geometry
            CanvasGeometry canvasGeometry= CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);
            drawingSession.FillGeometry(canvasGeometry, Windows.UI.Color.FromArgb(90, 54, 135, 230));
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
        }
    }
}