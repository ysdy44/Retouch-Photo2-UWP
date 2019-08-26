using FanKit.Win2Ds;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// <see cref="PenTool"/>'s PenMoveSingleNodePointTool.
    /// </summary>
    public class MoveSingleNodePointTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        Vector2 _left;
        Vector2 _right;
        bool _isSmooth;

        public void Start(Vector2 point, int selectedIndex)
        {
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                //Check the selected node.
                if (i == selectedIndex)
                {
                    Node node = this.Nodes[selectedIndex];
                    node.IsChecked = true;
                    this.Nodes[selectedIndex] = node;

                    this._left = node.LeftControlPoint - node.Point;
                    this._right = node.RightControlPoint - node.Point;
                    this._isSmooth = node.IsSmooth;
                }
                //Unchecked others.
                else
                {
                    Node node = this.Nodes[i];
                    if (node.IsChecked)
                    {
                        node.IsChecked = false;
                        this.Nodes[i] = node;
                    }
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 point, int selectedIndex)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            Node node = new Node
            {
                Point = canvasPoint,
                LeftControlPoint = canvasPoint + this._left,
                RightControlPoint = canvasPoint + this._right,
                IsChecked = true,
                IsSmooth = this._isSmooth,
            };
            this.Nodes[selectedIndex] = node;

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 point, int selectedIndex)
        {
            this.Delta(point, selectedIndex);
            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
        }
    }
}