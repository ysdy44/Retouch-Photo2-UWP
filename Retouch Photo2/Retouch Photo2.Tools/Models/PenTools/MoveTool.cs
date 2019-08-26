using FanKit.Win2Ds;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenMoveTool.
    /// </summary>
    public class MoveTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        public void Start()
        {
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                Node node = this.Nodes[i];
                if (node.IsChecked)
                {
                    node.CacheTransform();
                    this.Nodes[i] = node;
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Vector2 vector = canvasPoint - canvasStartingPoint;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                Node node = this.Nodes[i];
                if (this.Nodes[i].IsChecked)
                {
                    node.TransformAdd(vector);
                    this.Nodes[i] = node;
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point)
        {
            this.Delta(startingPoint, point);
            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
        }
    }
}