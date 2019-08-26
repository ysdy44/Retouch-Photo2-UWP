using FanKit.Transformers;
using FanKit.Win2Ds;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Models.PenTools;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// State of <see cref="PenTool"/>. 
    /// </summary>
    public enum PenToolMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Preview a line before creating a layer. </summary>
        Preview,
        /// <summary> Add a node to nodes in curve layer. </summary>
        Add,
        /// <summary> Move multiple nodes in curve layer. </summary>
        Move,

        /// <summary> Move a node's point. </summary>
        MoveSingleNodePoint,
        /// <summary> Move a node's left-control-point. </summary>
        MoveSingleNodeLeftControlPoint,
        /// <summary> Move a node's right-control-point. </summary>
        MoveSingleNodeRightControlPoint,

        /// <summary> Draw a choose rectangle. </summary>
        RectChoose,
    }

    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public class PenTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        List<Node> Nodes => this.SelectionViewModel.CurveLayer.Nodes;

        //Pen
        public PenToolMode Mode = PenToolMode.None;
        public readonly PreviewTool PreviewTool = new PreviewTool();
        public readonly AddTool AddTool = new AddTool();
        public readonly MoveTool MoveTool = new MoveTool();
        public readonly Guider Guider = new Guider();
        public readonly MoveSingleNodePointTool MoveSingleNodePointTool = new MoveSingleNodePointTool();
        public readonly MoveSingleNodeControlPointTool MoveSingleNodeControlPointTool = new MoveSingleNodeControlPointTool();
        public readonly RectChooseTool RectChooseTool = new RectChooseTool();

        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenControl();
        public FrameworkElement ShowIcon { get; } = new PenControl();
        public Page Page => this._penPage;
        PenPage _penPage { get; } = new PenPage();

        public void Starting(Vector2 point)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                this.Mode = PenToolMode.Preview;
                this.PreviewTool.Start(point);//PreviewNode
                return;
            }

            if (this.SelectionViewModel.IsPenToolNodesMode == false)
            {
                this.Mode = PenToolMode.Add;
                this.AddTool.Start(point);//AddNode
                return;
            }

            //Guider
            this.Guider.SetType(point);
            switch (this.Guider.Type)
            {
                case GuiderType.PointWithChecked:
                    {
                        this.Mode = PenToolMode.Move;
                        this.MoveTool.Start();//EditMove
                        return;
                    }
                case GuiderType.PointWithoutChecked:
                    {
                        this.Mode = PenToolMode.MoveSingleNodePoint;
                        this.MoveSingleNodePointTool.Start(point, this.Guider.Index);//MoveSingleNodePointTool
                        return;
                    }
                case GuiderType.LeftControlPoint:
                    {
                        this.Mode = PenToolMode.MoveSingleNodeLeftControlPoint;
                        this.MoveSingleNodeControlPointTool.StartLeftControl(this.Guider.Index);//MoveSingleNodeControlPointTool
                        return;
                    }
                case GuiderType.RightControlPoint:
                    {
                        this.Mode = PenToolMode.MoveSingleNodeRightControlPoint;
                        this.MoveSingleNodeControlPointTool.StartRightControl(this.Guider.Index);//MoveSingleNodeControlPointTool
                        return;
                    }
            }

            this.Mode = PenToolMode.RectChoose;
            this.RectChooseTool.Start(point);//RectChoose
            return;
        }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == PenToolMode.Preview)
                {
                    this.PreviewTool.Delta(point);//PreviewNode
                }
                return;
            }

            switch (this.Mode)
            {
                case PenToolMode.Add:
                    this.AddTool.Delta(point);//AddNode
                    return;
                case PenToolMode.Move:
                    this.MoveTool.Delta(startingPoint, point);//EditMove
                    return;

                case PenToolMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointTool.Delta(point, this.Guider.Index);//MoveSingleNodePointTool
                    return;
                case PenToolMode.MoveSingleNodeLeftControlPoint:
                    this.MoveSingleNodeControlPointTool.DeltaLeftControl(point, this.Guider.Index);//MoveSingleNodeControlPointTool
                    return;
                case PenToolMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointTool.DeltaRightControl(point, this.Guider.Index);//MoveSingleNodeControlPointTool
                    return;

                case PenToolMode.RectChoose:
                    this.RectChooseTool.Delta(startingPoint, point);//RectChoose
                    return;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            PenToolMode mode = this.Mode;
            this.Mode = PenToolMode.None;

            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (mode == PenToolMode.Preview)
                {
                    this.PreviewTool.Complete(startingPoint, point, isSingleStarted);//PreviewNode
                }
                return;
            }

            if (mode == PenToolMode.Add && this.SelectionViewModel.CurveLayer != null)
            {
                this.AddTool.Complete(point);//AddNode
                return;
            }

            if (isSingleStarted)
            {
                switch (mode)
                {
                    case PenToolMode.Move:
                        this.MoveTool.Complete(startingPoint, point);//EditMove
                        return;

                    case PenToolMode.MoveSingleNodePoint:
                        this.MoveSingleNodePointTool.Complete(point, this.Guider.Index);//MoveSingleNodePointTool
                        return;
                    case PenToolMode.MoveSingleNodeLeftControlPoint:
                        this.MoveSingleNodeControlPointTool.CompleteLeftControl(point, this.Guider.Index);//MoveSingleNodeControlPointTool
                        return;
                    case PenToolMode.MoveSingleNodeRightControlPoint:
                        this.MoveSingleNodeControlPointTool.CompleteRightControl(point, this.Guider.Index);//MoveSingleNodeControlPointTool
                        return;

                    case PenToolMode.RectChoose:
                        this.RectChooseTool.Complete(startingPoint, point);//RectChoose
                        return;
                }
            }

            this.ViewModel.Invalidate();//Invalidate
            return;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == PenToolMode.Preview)
                {
                    this.PreviewTool.Draw(drawingSession);//PreviewNode
                }
                return;
            }

            switch (this.Mode)
            {
                case PenToolMode.Add:
                    this.AddTool.Draw(drawingSession);//AddNode
                    break;

                case PenToolMode.RectChoose:
                    this.RectChooseTool.Draw(drawingSession);//RectChoose
                    break;
            }

            //Draw end-point and control points.
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                for (int i = 0; i < this.Nodes.Count; i++)
                {
                    Node node = this.Nodes[i];
                    Vector2 vector = Vector2.Transform(node.Point, matrix);

                    if (node.IsChecked == false)
                    {
                        if (node.IsSmooth == false) drawingSession.DrawNode3(vector);
                        else drawingSession.DrawNode(vector);
                    }
                    else
                    {
                        if (node.IsSmooth == false) drawingSession.DrawNode4(vector);
                        else
                        {
                            //Ignoring the right-control-point of the first point.
                            if (i != 0)
                            {
                                Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);
                                drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                                drawingSession.DrawNode(rightControlPoint);//TODO: Fankit更新后，请改成DrawNode5
                                drawingSession.FillCircle(rightControlPoint, 3, Windows.UI.Colors.Red);
                            }

                            //Ignoring the left-control-point of the last point.
                            if (i != this.Nodes.Count - 1)
                            {
                                Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                                drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                                drawingSession.DrawNode(leftControlPoint);//TODO: Fankit更新后，请改成DrawNode5
                            }

                            drawingSession.DrawNode2(vector);
                        }
                    }
                }
            }
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            if (this.SelectionViewModel.CurveLayer == null) return;

            //The PenTool may change the current CurveLayer's transformer.
            this.SelectionViewModel.Transformer = this.SelectionViewModel.CurveLayer.Destination;
        }
    }
}