using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{

    /// <summary>
    /// <see cref="ITool"/>'s NodeTool.
    /// </summary>
    public partial class NodeTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
                this.Title = resource.GetString("/Tools/Node");

            this.RemoveTextBlock.Text = resource.GetString("/Tools/Node_Remove");
            this.InsertTextBlock.Text = resource.GetString("/Tools/Node_Insert");
            this.SharpTextBlock.Text = resource.GetString("/Tools/Node_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("/Tools/Node_Smooth");
        }


        //@Content
        public ToolType Type => ToolType.Node;
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new NodeIcon();
        readonly ToolButton _button = new ToolButton(new NodeIcon());


        Layerage Layerage;
        NodeCollectionMode NodeCollectionMode;
        TransformerRect TransformerRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            this.Layerage = this.GetNodeCollectionLayer(startingPoint, matrix);

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Move:
                    {
                        if (this.Layerage == null) break;

                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                layer.Nodes.CacheTransformOnlySelected();
                            }
                        });

                        {
                            //Selection
                            ILayer layer = this.Layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                //Snap
                                if (this.IsSnap) this.ViewModel.VectorVectorSnapInitiate(layer.Nodes);
                            }
                        }
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    {
                        if (this.Layerage == null) break;

                        //Selection
                        ILayer layer = this.Layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            layer.Nodes.SelectionOnlyOne(startingPoint, matrix);

                            //Snap
                            if (this.IsSnap) this.ViewModel.VectorVectorSnapInitiate(layer.Nodes);
                        }
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    {
                        if (this.Layerage == null) break;

                        //Selection
                        ILayer layer = this.Layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            layer.Nodes.SelectedItem.CacheTransform();
                        }
                    }
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                        this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);

                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                layer.Nodes.CacheTransform();
                            }
                        });
                    }
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Move:
                    {
                        if (this.Layerage == null) break;

                        //Snap
                        if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);
                        Vector2 canvasMove = canvasPoint - canvasStartingPoint;

                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layerage.RefactoringParentsRender();
                                layer.Nodes.TransformAddOnlySelected(canvasMove);
                            }
                        });
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    {
                        if (this.Layerage == null) break;
                        ILayer layer = this.Layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            Node node = layer.Nodes.SelectedItem;

                            //Snap
                            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            this.Layerage.RefactoringParentsRender();
                            Node.Move(canvasPoint, node);
                        }
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    {
                        if (this.Layerage == null) break;
                        ILayer layer = this.Layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            Node node = layer.Nodes.SelectedItem;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            this.Layerage.RefactoringParentsRender();
                            Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: true);
                        }
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    {
                        if (this.Layerage == null) break;
                        ILayer layer = this.Layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            Node node = layer.Nodes.SelectedItem;

                            //Refactoring
                            layer.IsRefactoringRender = true;
                            this.Layerage.RefactoringParentsRender();
                            Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: false);
                        }
                    }
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);

                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                layer.Nodes.BoxChoose(this.TransformerRect);
                            }
                        });
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (isOutNodeDistance)
            {
                switch (this.NodeCollectionMode)
                {
                    case NodeCollectionMode.Move:
                        {
                            if (this.Layerage == null) break;

                            //Snap
                            if (this.IsSnap)
                            {
                                canvasPoint = this.Snap.Snap(canvasPoint);
                                this.Snap.Default();
                            }
                            Vector2 canvasMove = canvasPoint - canvasStartingPoint;
                       
                            //History
                            LayersPropertyHistory history = new LayersPropertyHistory("Move nodes");
                            
                            //Selection
                            this.SelectionViewModel.SetValue((layerage) =>
                            {
                                ILayer layer = layerage.Self;

                                if (layer.Type == LayerType.Curve)
                                {
                                    //History
                                    var previous = layer.Nodes.NodesStartingClone().ToList();
                                    history.UndoActions.Push(() =>
                                    {
                                        //Refactoring
                                        layer.IsRefactoringTransformer = true;
                                        layer.IsRefactoringRender = true;
                                        layer.IsRefactoringIconRender = true;
                                        layer.Nodes.NodesReplace(previous);
                                    });

                                    //Refactoring
                                    layer.IsRefactoringTransformer = true;
                                    layer.IsRefactoringRender = true;
                                    layer.IsRefactoringIconRender = true;
                                    layerage.RefactoringParentsTransformer();
                                    layerage.RefactoringParentsRender();
                                    layerage.RefactoringParentsIconRender();
                                    layer.Nodes.TransformAddOnlySelected(canvasMove);
                                }
                            });

                            //History
                            this.ViewModel.HistoryPush(history);
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        {
                            if (this.Layerage == null) break;
                            ILayer layer = this.Layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                Node node = layer.Nodes.SelectedItem;

                                //Snap
                                if (this.IsSnap)
                                {
                                    canvasPoint = this.Snap.Snap(canvasPoint);
                                    this.Snap.Default();
                                }

                                //History
                                LayersPropertyHistory history = new LayersPropertyHistory("Move node");

                                var previous = layer.Nodes.Index;
                                var previous1 = node.Clone();
                                history.UndoActions.Push(() =>
                                {
                                    //Refactoring
                                    layer.IsRefactoringTransformer = true;
                                    layer.IsRefactoringRender = true;
                                    layer.IsRefactoringIconRender = true;
                                    layer.Nodes[previous] = previous1;
                                });

                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                this.Layerage.RefactoringParentsTransformer();
                                this.Layerage.RefactoringParentsRender();
                                this.Layerage.RefactoringParentsIconRender();
                                Node.Move(canvasPoint, node);

                                //History
                                this.ViewModel.HistoryPush(history);
                            }
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        {
                            if (this.Layerage == null) break;
                            ILayer layer = this.Layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                Node node = layer.Nodes.SelectedItem;

                            //History
                            LayersPropertyHistory history = new LayersPropertyHistory("Move node control point");

                                var previous = layer.Nodes.Index;
                                var previous1 = node.StartingLeftControlPoint;
                                var previous2 = node.StartingRightControlPoint;
                                history.UndoActions.Push(() =>
                                {
                                    Node node2 = layer.Nodes[previous];

                                    //Refactoring
                                    layer.IsRefactoringTransformer = true;
                                    layer.IsRefactoringRender = true;
                                    layer.IsRefactoringIconRender = true;
                                    node2.LeftControlPoint = previous1;
                                    node2.RightControlPoint = previous2;
                                });

                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                this.Layerage.RefactoringParentsTransformer();
                                this.Layerage.RefactoringParentsRender();
                                this.Layerage.RefactoringParentsIconRender();
                                Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: true);
                            
                            //History
                            this.ViewModel.HistoryPush(history);
                            }       
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        {
                            if (this.Layerage == null) break;
                            ILayer layer = this.Layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                Node node = layer.Nodes.SelectedItem;

                                //History
                                LayersPropertyHistory history = new LayersPropertyHistory("Move node control point");

                                var previous = layer.Nodes.Index;
                                var previous1 = node.StartingLeftControlPoint;
                                var previous2 = node.StartingRightControlPoint;
                                history.UndoActions.Push(() =>
                                {
                                    Node node2 = layer.Nodes[previous];

                                    //Refactoring
                                    layer.IsRefactoringTransformer = true;
                                    layer.IsRefactoringRender = true;
                                    layer.IsRefactoringIconRender = true;
                                    node2.LeftControlPoint = previous1;
                                    node2.RightControlPoint = previous2;
                                });

                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                this.Layerage.RefactoringParentsTransformer();
                                this.Layerage.RefactoringParentsRender();
                                this.Layerage.RefactoringParentsIconRender();
                                Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: false);

                            //History
                            this.ViewModel.HistoryPush(history);
                            }
                        }
                        break;
                    case NodeCollectionMode.RectChoose:
                        {
                            this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                            
                            //History
                            LayersPropertyHistory history = new LayersPropertyHistory("Set nodes is checked");
                                                                                                          
                            //Selection
                            this.SelectionViewModel.SetValue((layerage) =>
                            {
                                ILayer layer = layerage.Self;

                                if (layer.Type == LayerType.Curve)
                                {
                                    layer.Nodes.BoxChoose(this.TransformerRect);

                                    //History
                                    var previous = layer.Nodes.NodesStartingClone().ToList();
                                    history.UndoActions.Push(() =>
                                    {
                                        //Refactoring
                                        layer.IsRefactoringTransformer = true;
                                        layer.IsRefactoringRender = true;
                                        layer.IsRefactoringIconRender = true;
                                        layer.Nodes.NodesReplace(previous);
                                    });
                                }
                            });

                            //History
                            this.ViewModel.HistoryPush(history);
                        }
                        break;
                }
            }


            this.NodeCollectionMode = NodeCollectionMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set nodes is checked");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    layer.Nodes.CacheTransform();
                    layer.Nodes.SelectionOnlyOne(point, matrix);

                    //History
                    var previous = layer.Nodes.NodesStartingClone().ToList();
                    history.UndoActions.Push(() =>
                    {
                        //Refactoring
                        layer.Nodes.NodesReplace(previous);
                    });
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                    {
                        ILayer layer = this.SelectionViewModel.SelectionLayerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                case NodeCollectionMode.MoveSingleNodePoint:
                    {
                        //Snapping
                        if (this.IsSnap)
                        {
                            this.Snap.Draw(drawingSession, matrix);
                            this.Snap.DrawNode2(drawingSession, matrix);
                        }
                    }
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        CanvasGeometry canvasGeometry = this.TransformerRect.ToRectangle(this.ViewModel.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }
        }

    }
}