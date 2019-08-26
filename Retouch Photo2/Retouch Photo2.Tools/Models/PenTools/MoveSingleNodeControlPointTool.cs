using FanKit.Transformers;
using FanKit.Win2Ds;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models.PenTools
{
    public enum SelfControlPointMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> No change the angle. </summary>
        Angle,
        /// <summary> No change the length. </summary>
        Length,
    }

    /// <summary>
    /// 长度：不等，相等，等比
    /// </summary>
    public enum EachControlPointLengthMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Equal length. </summary>
        Equal,
        /// <summary> Ratio length. </summary>
        Ratio,
    }
    /// <summary>
    /// 角度：不等，相等，角度不变
    /// </summary>
    public enum EachControlPointAngleMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Origin symmetry. </summary>
        Asymmetric,
        /// <summary> Fixe angle. </summary>
        Fixed,
    }

    /// <summary>
    /// <see cref="PenTool"/>'s PenMoveSingleNodeControlPointTool.
    /// </summary>
    public class MoveSingleNodeControlPointTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;


        Node _oldNode;

        public SelfControlPointMode Mode;

        public EachControlPointLengthMode LengthMode;
        public EachControlPointAngleMode AngleMode;


        public void StartLeftControl(int selectedIndex) => this._oldNode = this.Nodes[selectedIndex];
        public void DeltaLeftControl(Vector2 point, int selectedIndex) => this._setSelectedNode(false, point, selectedIndex);
        public void CompleteLeftControl(Vector2 point, int selectedIndex)
        {
              this.DeltaLeftControl(point, selectedIndex);
            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
        }

        public void StartRightControl(int selectedIndex) => this._oldNode = this.Nodes[selectedIndex];
        public void DeltaRightControl(Vector2 point, int selectedIndex) => this._setSelectedNode(true, point, selectedIndex);
        public void CompleteRightControl(Vector2 point, int selectedIndex)
        {
           this.DeltaRightControl(point, selectedIndex);
            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
        }


        private void _setSelectedNode(bool isRightControl, Vector2 point, int selectedIndex)
        {
            if (isRightControl==false)
            {
                var (newSelfControlPoint, newEachControlPoint) = MoveSingleNodeControlPointTool. _getNewNode(this.Mode, this.LengthMode, this.AngleMode, point, this._oldNode.Point, this._oldNode.LeftControlPoint, this._oldNode.RightControlPoint);
                this.Nodes[selectedIndex] = new Node
                {
                    Point = this._oldNode.Point,
                    LeftControlPoint = newSelfControlPoint,
                    RightControlPoint = newEachControlPoint,
                    IsChecked = true,
                    IsSmooth = true
                };
            }
            else
            {
                var (newSelfControlPoint, newEachControlPoint) = MoveSingleNodeControlPointTool._getNewNode(this.Mode, this.LengthMode, this.AngleMode, point, this._oldNode.Point, this._oldNode.RightControlPoint, this._oldNode.LeftControlPoint);
                this.Nodes[selectedIndex] = new Node
                {
                    Point = this._oldNode.Point,
                    LeftControlPoint = newEachControlPoint,
                    RightControlPoint = newSelfControlPoint,
                    IsChecked = true,
                    IsSmooth = true
                };
            }
            this.ViewModel.Invalidate();//Invalidate
        }


        //@Static
        private static (Vector2 newSelfControlPoint, Vector2 newEachControlPoint) _getNewNode(SelfControlPointMode mode, EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 vector, Vector2 oldPoint, Vector2 oldSelfControlPoint, Vector2 oldEachControlPoint)
        {
            Vector2 newSelfControlPoint = MoveSingleNodeControlPointTool._getNewSelfControlPoint(mode, vector, oldPoint, oldSelfControlPoint);
            Vector2 newEachControlPoint = MoveSingleNodeControlPointTool._getNewEachControlPoint(lengthMode, angleMode, oldPoint, newSelfControlPoint - oldPoint, oldSelfControlPoint - oldPoint, oldEachControlPoint - oldPoint);
            return (newSelfControlPoint, newEachControlPoint);
        }


        #region Self

        private static Vector2 _getNewSelfControlPoint(SelfControlPointMode mode, Vector2 prePoint, Vector2 oldPoint, Vector2 oldSelfControlPoint)
        {
            switch (mode)
            {
                case SelfControlPointMode.None: return prePoint;
                case SelfControlPointMode.Angle:
                    {
                        Vector2 newSelfControlPoint = TransformerMath.FootPoint(prePoint, oldPoint, oldSelfControlPoint);
                        return newSelfControlPoint;
                    }
                case SelfControlPointMode.Length:
                    {
                        Vector2 oldSelfVector = oldSelfControlPoint - oldPoint;
                        Vector2 preSelfVector = prePoint - oldPoint;

                        float oldSelfLength = oldSelfVector.Length();
                        float preSelfLength = preSelfVector.Length();

                        Vector2 oldSelfUnit = oldSelfVector / oldSelfLength;
                        Vector2 preSelfUnit = preSelfVector / preSelfLength;

                        Vector2 newSelfControlPoint = oldPoint + oldSelfLength * preSelfUnit;
                        return newSelfControlPoint;
                    }
            }
            return prePoint;
        }

        #endregion


        #region Each

        private static Vector2 _getNewEachControlPoint(EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 oldPoint, Vector2 newSelfVector, Vector2 oldSelfVector, Vector2 oldEachVector)
        {
            float newSelfLength = newSelfVector.Length();
            float oldSelfLength = oldSelfVector.Length();
            float oldEachLength = oldEachVector.Length();

            float newEachLength = MoveSingleNodeControlPointTool._getNewEachLength(lengthMode, newSelfLength, oldSelfLength, oldEachLength);
            Vector2 newEachUnit = MoveSingleNodeControlPointTool._getNewEachUnit(angleMode, newSelfVector / newSelfLength, oldSelfVector / newSelfLength, oldEachVector / oldEachLength);

            Vector2 newEachControlPoint = oldPoint + newEachLength * newEachUnit;
            return newEachControlPoint;
        }

        private static float _getNewEachLength(EachControlPointLengthMode lengthMode, float newSelfLength, float oldSelfLength, float oldEachLength)
        {
            switch (lengthMode)
            {
                case EachControlPointLengthMode.None: return oldEachLength;
                case EachControlPointLengthMode.Equal: return newSelfLength;
                case EachControlPointLengthMode.Ratio: return newSelfLength / oldSelfLength * oldEachLength;
            }
            return 0;
        }

        private static Vector2 _getNewEachUnit(EachControlPointAngleMode angleMode, Vector2 newSelfUnit, Vector2 oldSelfUnit, Vector2 oldEachUnit)
        {
            switch (angleMode)
            {
                case EachControlPointAngleMode.None: return oldEachUnit;
                case EachControlPointAngleMode.Asymmetric: return -newSelfUnit;
                case EachControlPointAngleMode.Fixed:
                    {
                        float newSelfRadians = TransformerMath.VectorToRadians(newSelfUnit);
                        float oldSelfRadians = TransformerMath.VectorToRadians(oldSelfUnit);
                        float oldEachRadians = TransformerMath.VectorToRadians(oldEachUnit);

                        float radians = newSelfRadians - oldSelfRadians + oldEachRadians;
                        return new Vector2((float)System.Math.Cos(radians), (float)System.Math.Sin(radians));
                    }
            }
            return Vector2.Zero;
        }

        #endregion
    }
}