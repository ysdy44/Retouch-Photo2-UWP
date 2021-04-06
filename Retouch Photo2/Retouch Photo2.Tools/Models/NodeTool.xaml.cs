// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Elements;
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
    public partial class NodeTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content
        public ToolType Type => ToolType.Node;
        public ToolGroupType GroupType => ToolGroupType.Tool;
        public string Title => this.NodePage.Title;
        public ControlTemplate Icon => this.NodePage.Icon;
        public FrameworkElement Page => this.NodePage;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.NodePage.MoreNodeToolTip.IsOpen; set => this.NodePage.MoreNodeToolTip.IsOpen = value; }
        readonly NodePage NodePage = new NodePage();


        Layerage Layerage;
        NodeCollectionMode NodeCollectionMode;
        TransformerRect TransformerRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            this.Layerage = this.GetNodeCollectionLayer(startingPoint, matrix);
            if (this.Layerage == null)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.RectChoose;
                this.TipViewModel.Cursor_ManipulationStarted_Tool();
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                return;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                    this.MoveStarted();
                    this.TipViewModel.Cursor_ManipulationStarted_Tool();
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointStarted(startingPoint, matrix);
                    this.TipViewModel.Cursor_ManipulationStarted_Tool();
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointStarted();
                    this.TipViewModel.Cursor_ManipulationStarted_Tool();
                    break;
                case NodeCollectionMode.RectChoose:
                    this.RectChooseStarted(startingPoint, point);
                    this.TipViewModel.Cursor_ManipulationStarted_Tool();
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.Layerage == null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.ViewModel.Invalidate();//Invalidate
                return;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                    this.MoveDelta(canvasStartingPoint, canvasPoint);
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointDelta(canvasPoint);
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    this.MoveSingleNodeControlPointDelta(canvasPoint, isLeftControlPoint: true);
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointDelta(canvasPoint, isLeftControlPoint: false);
                    break;
                case NodeCollectionMode.RectChoose:
                    this.RectChooseDelta(canvasStartingPoint, canvasPoint);
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            this.TipViewModel.Cursor_ManipulationStarted_None();

            if (this.Layerage == null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.None;
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                return;
            }

            if (isOutNodeDistance)
            {
                switch (this.NodeCollectionMode)
                {
                    case NodeCollectionMode.Move:
                        this.MoveComplete(canvasStartingPoint, canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        this.MoveSingleNodePointComplete(canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        this.MoveSingleNodeControlPointComplete(canvasPoint, isLeftControlPoint: true);
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        this.MoveSingleNodeControlPointComplete(canvasPoint, isLeftControlPoint: false);
                        break;
                    case NodeCollectionMode.RectChoose:
                        this.RectChooseComplete(canvasStartingPoint, canvasPoint);
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
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_MoveNode_IsChecked);

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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.Nodes.NodesReplace(previous);
                    };
                }
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }

        public void Cursor(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            NodeCollectionMode mode = this.GetNodeCollectionMode(point, matrix);
            switch (mode)
            {
                case NodeCollectionMode.None:
                    this.TipViewModel.Cursor_PointerEntered_None();
                    break;
                case NodeCollectionMode.Preview:
                    break;
                case NodeCollectionMode.Add:
                    break;
                case NodeCollectionMode.Move:
                    this.TipViewModel.Cursor_PointerEntered_Tool();
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.TipViewModel.Cursor_PointerEntered_Tool();
                    break;
                case NodeCollectionMode.RectChoose:
                    this.TipViewModel.Cursor_PointerEntered_None();
                    break;
                default:
                    this.TipViewModel.Cursor_PointerEntered_None();
                    break;
            }
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None: break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);
                        drawingSession.DrawNodeCollection(layer2.Nodes, matrix, this.ViewModel.AccentColor);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
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
                        CanvasGeometry canvasGeometry = this.TransformerRect.ToRectangle(LayerManager.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }

    }


    /// <summary>
    /// Page of <see cref="NodeTool"/>.
    /// </summary>
    internal partial class NodePage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        /// <summary> PenPage's Flyout. </summary>
        public NodeModeControl PenFlyout => this._PenFlyout;

        /// <summary> MoreNodeButton's ToolTip. </summary>
        public ToolTip MoreNodeToolTip => this._MoreNodeToolTip;


        //@Content 
        public string Title { get; private set; }
        public ControlTemplate Icon => this.IconContentControl.Template;


        //@Construct
        /// <summary>
        /// Initializes a NodePage. 
        /// </summary>
        public NodePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructNodes();
            this.ConstructSmooth();

            this.MoreNodeButton.Click += (s, e) =>
            {
                switch (this.SettingViewModel.DeviceLayoutType)
                {
                    case DeviceLayoutType.PC:
                        this._PenFlyout.Width = double.NaN;
                        this.Flyout.ShowAt(this.MoreNodeButton);
                        break;
                    case DeviceLayoutType.Pad:
                        this._PenFlyout.Width = double.NaN;
                        this.Flyout.ShowAt(this);
                        break;
                    case DeviceLayoutType.Phone:
                        this._PenFlyout.Width = this.ActualWidth - 40;
                        this.Flyout.ShowAt(this);
                        break;
                }
            };
        }

    }

    /// <summary>
    /// Page of <see cref="NodeTool"/>.
    /// </summary>
    internal partial class NodePage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Title = resource.GetString("Tools_Node");

            this.RemoveTextBlock.Text = resource.GetString("Tools_Node_Remove");
            this.InsertTextBlock.Text = resource.GetString("Tools_Node_Insert");
            this.SharpTextBlock.Text = resource.GetString("Tools_Node_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("Tools_Node_Smooth");

            this._MoreNodeToolTip.Content = resource.GetString("Tools_Node_MoreNode");
        }

        private void ConstructNodes()
        {
            this.RemoveButton.Click += (s, e) =>
            {
                IList<Layerage> removeLayerage = new List<Layerage>();


                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_RemoveNodes);

                    //Selection
                    this.SelectionViewModel.SetValue((layerage) =>
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            NodeBorderCollection nodeBorderCollection = new NodeBorderCollection(layer.Nodes);
                            NodeRemoveMode removeMode = nodeBorderCollection.GetRemoveMode();

                            switch (removeMode)
                            {
                                case NodeRemoveMode.RemoveCurve:
                                    {
                                        removeLayerage.Add(layerage);
                                    }
                                    break;

                                case NodeRemoveMode.RemovedNodes:
                                    {
                                        var previous = layer.Nodes.NodesClone().ToList();
                                        history.UndoAction += () =>
                                        {
                                            //Refactoring
                                            layer.IsRefactoringTransformer = true;
                                            layer.IsRefactoringRender = true;
                                            layer.IsRefactoringIconRender = true;
                                            layerage.RefactoringParentsTransformer();
                                            layerage.RefactoringParentsRender();
                                            layerage.RefactoringParentsIconRender();
                                            layer.Nodes.NodesReplace(previous);
                                        };

                                        //Refactoring
                                        layer.IsRefactoringTransformer = true;
                                        layer.IsRefactoringRender = true;
                                        layer.IsRefactoringIconRender = true;
                                        layerage.RefactoringParentsTransformer();
                                        layerage.RefactoringParentsRender();
                                        layerage.RefactoringParentsIconRender();
                                        IEnumerable<Node> uncheckedNodes = nodeBorderCollection.GetUnCheckedNodes();
                                        layer.Nodes.NodesReplace(uncheckedNodes);
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    });

                    //History
                    this.ViewModel.HistoryPush(history);
                }


                //Remove
                if (removeLayerage.Count != 0)
                {
                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_RemoveLayers);
                    this.ViewModel.HistoryPush(history);

                    foreach (Layerage remove in removeLayerage)
                    {
                        LayerManager.Remove(remove);
                    }

                    //Selection
                    this.SelectionViewModel.SetMode();//Selection
                    LayerManager.ArrangeLayers();
                }

                this.ViewModel.Invalidate();//Invalidate
            };


            this.InsertButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_InsertNodes);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        var previous = layer.Nodes.NodesStartingClone().ToList();
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            layer.IsRefactoringTransformer = true;
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layerage.RefactoringParentsTransformer();
                            layerage.RefactoringParentsRender();
                            layerage.RefactoringParentsIconRender();
                            layer.Nodes.NodesReplace(previous);
                        };

                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        NodeCollection.InterpolationCheckedNodes(layer.Nodes);
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        private void ConstructSmooth()
        {
            this.SharpButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_SharpNodes);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        layer.Nodes.CacheTransformOnlySelected();
                        bool isSuccessful = NodeCollection.SharpCheckedNodes(layer.Nodes);

                        if (isSuccessful)
                        {
                            //History
                            var previous = layer.Nodes.NodesStartingClone().ToList();
                            history.UndoAction += () =>
                            {
                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                layer.Nodes.NodesReplace(previous);
                            };
                        }
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };


            this.SmoothButton.Click += (s, e) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_SmoothNodes);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        layer.Nodes.CacheTransformOnlySelected();
                        bool isSuccessful = NodeCollection.SmoothCheckedNodes(layer.Nodes);

                        if (isSuccessful)
                        {
                            //History
                            var previous = layer.Nodes.NodesStartingClone().ToList();
                            history.UndoAction += () =>
                            {
                                //Refactoring
                                layer.IsRefactoringTransformer = true;
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                layer.Nodes.NodesReplace(previous);
                            };
                        }
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}