using FanKit.Win2Ds;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models.PenTools;

namespace Retouch_Photo2.Tools.Models
{
    public enum PenToolMode
    {
        Normal,

        PreviewNode,
        AddNode,
        EditMove,
        RectChoose,
    }

    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public class PenTool : ITool
    {
        //@PenModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;

        //Brush
        private PenToolMode Mode = PenToolMode.Normal;
        readonly PreviewNodeTool PreviewNodeTool = new PreviewNodeTool();
        readonly AddNodeTool AddNodeTool = new AddNodeTool();
        readonly EditMoveTool EditMoveTool = new EditMoveTool();
        readonly RectChooseTool RectChooseTool = new RectChooseTool();

        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenControl();
        public FrameworkElement ShowIcon { get; } = new PenControl();
        public Page Page { get; } = new PenPage();


        public void Starting(Vector2 point)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                this.Mode = PenToolMode.PreviewNode;
                this.PreviewNodeTool.Start(point);//PreviewNode
                return;
            }

            if (this.SelectionViewModel.IsPenToolNodeMode == false)
            {
                this.Mode = PenToolMode.AddNode;
                this.AddNodeTool.Start(point);//AddNode
                return;
            }

            if(false)
            {
                //    float nodeRadius = FanKit.Transformers.NodeRadius;
                float nodeRadius = 12.0f;
                float scalingNodeRadius = nodeRadius / this.ViewModel.CanvasTransformer.Scale;
                float scalingNodeRadiusSquare = scalingNodeRadius * scalingNodeRadius;
                foreach (Node node in this.SelectionViewModel.CurveLayer.Nodes)
                {
                    bool inNodeRadius = Vector2.DistanceSquared(node.Vector, point) < scalingNodeRadiusSquare;
                    if (inNodeRadius)
                    {
                        this.Mode = PenToolMode.EditMove;
                        this.EditMoveTool.Start(point);//EditMove
                        return;
                    }
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
                if (this.Mode == PenToolMode.PreviewNode)
                {
                    this.PreviewNodeTool.Delta(startingPoint, point);//PreviewNode
                }
                return;
            }

            switch (this.Mode)
            {
                case PenToolMode.AddNode:
                    this.AddNodeTool.Delta(startingPoint, point);//AddNode
                    break;
                case PenToolMode.EditMove:
                    this.EditMoveTool.Delta(startingPoint, point);//EditMove
                    break;
                case PenToolMode.RectChoose:
                    this.RectChooseTool.Delta(startingPoint, point);//RectChoose
                    break;
            } 
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            PenToolMode mode = this.Mode;
            this.Mode = PenToolMode.Normal;

            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (mode == PenToolMode.PreviewNode)
                {
                    this.PreviewNodeTool.Complete(startingPoint, point, isSingleStarted);//PreviewNode
                }
                return;
            }

            if (mode == PenToolMode.AddNode && this.SelectionViewModel.CurveLayer != null)
            {
                this.AddNodeTool.Complete(startingPoint, point, isSingleStarted);//AddNode
                return;
            }

            if (isSingleStarted)
            {
                    switch (mode)
                    {
                        case PenToolMode.EditMove:
                            {
                                this.EditMoveTool.Complete(startingPoint, point, isSingleStarted);//EditMove
                                return;
                            }
                        case PenToolMode.RectChoose:
                            {
                                this.RectChooseTool.Complete(startingPoint, point, isSingleStarted);//RectChoose
                                return;
                            }
                    }
            }

            this.ViewModel.SetText();
            this.ViewModel.Invalidate();//Invalidate
            return;
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == PenToolMode.PreviewNode)
                {
                    this.PreviewNodeTool.Draw(drawingSession);//PreviewNode
                }
                return;
            }

            switch (this.Mode)
            {
                case PenToolMode.AddNode:
                    this.AddNodeTool.Draw(drawingSession);//AddNode
                    break;
                case PenToolMode.RectChoose:
                    this.RectChooseTool.Draw(drawingSession);//RectChoose
                    break;
            }

            this.EditMoveTool.Draw(drawingSession);//EditMove
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}