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

        TransformerRect _rect;
        bool _isPressed = false;

        public void Start(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this._rect = new TransformerRect(canvasPoint, canvasPoint);
            this._isPressed = true;
            
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint=Vector2.Transform(point, inverseMatrix);

            this._rect = new TransformerRect(canvasStartingPoint, canvasPoint);

            //Choose point which in the rect.
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                Node node = this.Nodes[i];

                bool isContained = node.Contained(this._rect);
                if (node.IsChecked != isContained)
                {
                    node.IsChecked = isContained;
                    this.Nodes[i] = node;
                }
            }
            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point)
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

            //TODO:替换Fankit的方法，drawingSession.DrawRect
            {
                //LTRB
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                Vector2 leftTop = Vector2.Transform(this._rect.LeftTop, matrix);
                Vector2 rightTop = Vector2.Transform(this._rect.RightTop, matrix);
                Vector2 rightBottom = Vector2.Transform(this._rect.RightBottom, matrix);
                Vector2 leftBottom = Vector2.Transform(this._rect.LeftBottom, matrix);

                //Points
                Vector2[] points = new Vector2[]
                {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
                };

                //Geometry
                CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);
                drawingSession.FillGeometry(canvasGeometry, Windows.UI.Color.FromArgb(90, 54, 135, 230));
                drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
            }
        }
    }
}