using FanKit.Transformers;
using FanKit.Win2Ds;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    /// <summary>
    /// State of <see cref="Guider"/>. 
    /// </summary>
    public enum GuiderType
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Checked node's point. </summary>
        PointWithChecked,
        /// <summary> Unchecked node's point. </summary>
        PointWithoutChecked,

        /// <summary> left-control-point. </summary>
        LeftControlPoint,
        /// <summary> right-control-point. </summary>
        RightControlPoint,        
    }

    public class Guider
    {
        //@PenModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        public GuiderType Type { get; private set; } = GuiderType.None;
        public int Index { get; private set; }
        
        public void SetType(Vector2 point)
        {
            //InNodeRadius
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            float scalingNodeRadius = Transformer.NodeRadius / this.ViewModel.CanvasTransformer.Scale;
            float scalingNodeRadiusSquare = scalingNodeRadius * scalingNodeRadius;

            bool getInNodeRadius(Vector2 vector)
            {
                float distanceSquared = Vector2.DistanceSquared(vector, canvasPoint);
                bool inNodeRadius = distanceSquared < scalingNodeRadiusSquare;
                return inNodeRadius;
            }


            for (int i = 0; i < this.Nodes.Count; i++)
            {
                Node node = this.Nodes[i];

                if (node.IsChecked == false)
                {
                    //When you click on a unchecked node point ...      
                    if (getInNodeRadius(node.Point))
                    {
                        this.Index = i;
                        this.Type = GuiderType.PointWithoutChecked;
                        return;
                    }
                }
                else
                {
                    if (getInNodeRadius(node.LeftControlPoint))
                    {
                        //Ignoring the left-control-point of the last point.
                        if (i != this.Nodes.Count-1)
                        {
                            this.Index = i;
                            this.Type = GuiderType.LeftControlPoint;
                            return;
                        }
                    }
                    else if (getInNodeRadius(node.RightControlPoint))
                    {
                        //Ignoring the right-control-point of the first point.
                        if (i != 0)
                        {
                            this.Index = i;
                            this.Type = GuiderType.RightControlPoint;
                            return;
                        }
                    }

                    //When you click on a checked node point ...
                    if (getInNodeRadius(node.Point))
                    {
                        this.Type = GuiderType.PointWithChecked;
                        return;
                    }
                }
            }


            // None
            this.Type = GuiderType.None;
            return;
        }
    }
}