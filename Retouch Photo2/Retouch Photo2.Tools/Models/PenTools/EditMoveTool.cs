using FanKit.Transformers;
using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenEditMoveTool.
    /// </summary>
    public class EditMoveTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        public TransformerRect Rect;

        public void Start(Vector2 point)
        {
            foreach (Node node in this.Nodes)
            {
                if (node.IsChecked) node.CacheTransform();
            }
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Vector2 vector = canvasPoint - canvasStartingPoint;
            for (int i = 0; i < this.Nodes.Count - 1; i++)
            {
                if (this.Nodes[i].IsChecked)
                {
                    this.Nodes[i].TransformAdd(vector);
                }
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {

        }

        /// <summary>
        /// Draw end-point and control points.
        /// </summary>
        /// <param name="drawingSession"></param>
        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            foreach (Node item in this.Nodes)
            {
                Vector2 vector = Vector2.Transform(item.Vector, matrix);

                if (item.IsChecked)
                {
                    if (item.ChooseMode== NodeChooseMode.None) drawingSession.DrawNode4(vector);
                    else
                    {
                        Vector2 rightControl = Vector2.Transform(item.RightControl, matrix);
                        drawingSession.DrawLineDodgerBlue(vector, rightControl);
                        drawingSession.DrawNode2(vector);

                        Vector2 leftControl = Vector2.Transform(item.LeftControl, matrix);
                        drawingSession.DrawLineDodgerBlue(vector, leftControl);
                        drawingSession.DrawNode2(vector);

                        drawingSession.DrawNode(vector);
                    }
                }
                else
                {
                    if (item.ChooseMode == NodeChooseMode.None) drawingSession.DrawNode3(vector);
                    else drawingSession.DrawNode(vector);
                }
              
            }
        }
    }
}