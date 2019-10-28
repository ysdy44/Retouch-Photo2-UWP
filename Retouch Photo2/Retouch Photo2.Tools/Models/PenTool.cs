using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        GeometryCurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;
        NodeCollection Nodes => this.CurveLayer.Nodes;
        
        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenIcon();
        public IToolButton Button { get; } = new PenButton();
        public IToolPage Page => this._penPage;
        PenPage _penPage { get; } = new PenPage();

        //Pen
        public NodeCollectionMode Mode = NodeCollectionMode.None;
        Node _oldNode;
        TransformerRect _transformerRect;

        //Add
        Node _addEndNode;
        Node _addLastNode;

        public void Starting(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.CurveLayer == null)
                this.Mode = NodeCollectionMode.Preview;
            else if (this.SelectionViewModel.IsPenToolNodesMode == false)
                this.Mode = NodeCollectionMode.Add;
            else
                this.Mode = NodeCollection.ContainsNodeCollectionMode(point, this.Nodes, matrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewStart(canvasPoint);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        Node node = new Node
                        {
                            Point = canvasPoint,
                            LeftControlPoint = canvasPoint,
                            RightControlPoint = canvasPoint,
                            IsChecked = false,
                            IsSmooth = false,
                        };
                        this._addEndNode = node;
                        this._addLastNode = this.CurveLayer.Nodes.Last();
                    }
                    break;
                case NodeCollectionMode.Move:
                    this.Nodes.CacheTransform(isOnlySelected: true);
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.Nodes.SelectionOnlyOne(this.Nodes.Index);
                    this._oldNode = this.Nodes[this.Nodes.Index];
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    this._oldNode = this.Nodes[this.Nodes.Index];
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this._oldNode = this.Nodes[this.Nodes.Index];
                    break;
                case NodeCollectionMode.RectChoose:
                    this._transformerRect = new TransformerRect(canvasPoint, canvasPoint);
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewDelta(canvasPoint);//PreviewNode
                }
            }
            else
            {
                switch (this.Mode)
                {
                    case NodeCollectionMode.None:
                        break;
                    case NodeCollectionMode.Preview:
                        break;
                    case NodeCollectionMode.Add:
                        {
                            Node node = new Node
                            {
                                Point = canvasPoint,
                                LeftControlPoint = canvasPoint,
                                RightControlPoint = canvasPoint,
                                IsChecked = false,
                                IsSmooth = false,
                            };
                            this._addEndNode = node;
                        }
                        break;
                    case NodeCollectionMode.Move:
                        {
                            Vector2 vector = canvasPoint - canvasStartingPoint;
                            this.Nodes.TransformAdd(vector, isOnlySelected: true);
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        this.Nodes[this.Nodes.Index] = this._oldNode.Move(canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        this.Nodes[this.Nodes.Index] = this._penPage.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        this.Nodes[this.Nodes.Index] = this._penPage.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
                        break;
                    case NodeCollectionMode.RectChoose:
                        {
                            TransformerRect transformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                            this._transformerRect = transformerRect;
                            this.Nodes.RectChoose(transformerRect);
                        }
                        break;
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewComplete(canvasStartingPoint, canvasPoint, isSingleStarted);//PreviewNode
                }
            }
            else
            {
                if (this.Mode == NodeCollectionMode.Add && this.CurveLayer != null)
                {
                    Node node = new Node
                    {
                        Point = canvasPoint,
                        LeftControlPoint = canvasPoint,
                        RightControlPoint = canvasPoint,
                        IsChecked = false,
                        IsSmooth = false,
                    };
                    this.Nodes.Add(node);
                }
                else if (isSingleStarted)
                {
                    switch (this.Mode)
                    {
                        case NodeCollectionMode.Move:
                            {
                                Vector2 vector = canvasPoint - canvasStartingPoint;
                                this.Nodes.TransformAdd(vector, isOnlySelected: true);
                            }
                            break;
                        case NodeCollectionMode.MoveSingleNodePoint:
                            this.Nodes[this.Nodes.Index] = this._oldNode.Move(canvasPoint);
                            break;
                        case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                            this.Nodes[this.Nodes.Index] = this._penPage.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                            break;
                        case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                            this.Nodes[this.Nodes.Index] = this._penPage.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
                            break;
                        case NodeCollectionMode.RectChoose:
                            this._transformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                            break;
                    }
                }

                this.CurveLayer.IsRefactoringTransformer = true;//RefactoringTransformer
                this.Mode = NodeCollectionMode.None;
            }
            
            this.ViewModel.Invalidate();//Invalidate
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewDraw(drawingSession);//PreviewNode
                }
                return;
            }

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.Mode)
            {
                case NodeCollectionMode.Add:
                    {
                        Vector2 lastPoint = Vector2.Transform(this._addLastNode.Point, matrix);
                        Vector2 endPoint = Vector2.Transform(this._addEndNode.Point, matrix);

                        drawingSession.DrawLineDodgerBlue(lastPoint, endPoint);
                        drawingSession.DrawNode4(endPoint);
                    }
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        CanvasGeometry canvasGeometry = this._transformerRect.ToRectangle(this.ViewModel.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }

            drawingSession.DrawNodeCollection(this.Nodes, matrix);
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            if (this.CurveLayer == null) return;

            //The PenTool may change the current CurveLayer's transformer.
            Transformer transformer = this.CurveLayer.GetActualDestinationWithRefactoringTransformer;
            this.SelectionViewModel.Transformer = transformer;
        }
    }
}