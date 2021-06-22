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

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;


        //@Content
        public ToolType Type => ToolType.Node;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }
        public bool IsOpen { get => this.MoreToolTip.IsOpen; set => this.MoreToolTip.IsOpen = value; }


        //@Construct
        /// <summary>
        /// Initializes a NodeTool. 
        /// </summary>
        public NodeTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.RemoveButton.Tapped += (s, e) => this.Remove();
            this.InsertButton.Tapped += (s, e) => this.Insert();

            this.SharpButton.Tapped += (s, e) => this.Sharp();
            this.SmoothButton.Tapped += (s, e) => this.Smooth();

            this.MoreButton.Tapped += (s, e) =>
            {
                switch (this.SettingViewModel.DeviceLayoutType)
                {
                    case DeviceLayoutType.PC:
                        this.PenFlyout.Width = double.NaN;
                        this.Flyout.ShowAt(this.MoreButton);
                        break;
                    case DeviceLayoutType.Pad:
                        this.PenFlyout.Width = double.NaN;
                        this.Flyout.ShowAt(this);
                        break;
                    case DeviceLayoutType.Phone:
                        this.PenFlyout.Width = this.ActualWidth - 40;
                        this.Flyout.ShowAt(this);
                        break;
                }
            };
        }


        Layerage Layerage;
        NodeCollectionMode NodeCollectionMode;
        TransformerRect TransformerRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            this.Layerage = this.GetNodeCollectionLayer(startingPoint, matrix);
            if (this.Layerage is null)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.RectChoose;

                // Cursor
                CoreCursorExtension.IsManipulationStarted = true;
                CoreCursorExtension.Cross();

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail); // Invalidate
                return;
            }

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.Move:
                    this.MoveStarted();
                    // Cursor
                    CoreCursorExtension.IsManipulationStarted = true;
                    CoreCursorExtension.Cross();
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.MoveSingleNodePointStarted(startingPoint, matrix);
                    // Cursor
                    CoreCursorExtension.IsManipulationStarted = true;
                    CoreCursorExtension.Cross();
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.MoveSingleNodeControlPointStarted();
                    // Cursor
                    CoreCursorExtension.IsManipulationStarted = true;
                    CoreCursorExtension.Cross();
                    break;
                case NodeCollectionMode.RectChoose:
                    this.RectChooseStarted(startingPoint, point);
                    // Cursor
                    CoreCursorExtension.IsManipulationStarted = true;
                    CoreCursorExtension.Cross();
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail); // Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.Layerage is null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.ViewModel.Invalidate(); // Invalidate
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

            this.ViewModel.Invalidate(); // Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            // Cursor
            CoreCursorExtension.IsManipulationStarted = false;
            CoreCursorExtension.Cross();

            if (this.Layerage is null)
            {
                this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                this.NodeCollectionMode = NodeCollectionMode.None;
                this.ViewModel.Invalidate(InvalidateMode.HD); // Invalidate
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

            this.ViewModel.Invalidate(InvalidateMode.HD); // Invalidate
        }

        public void Clicke(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_MoveNode_IsChecked);

            // Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    layer.Nodes.CacheTransform();
                    layer.Nodes.SelectionOnlyOne(point, matrix);

                    // History
                    var previous = layer.Nodes.NodesStartingClone().ToList();
                    history.UndoAction += () =>
                    {
                        // Refactoring
                        layer.Nodes.NodesReplace(previous);
                    };
                }
            });

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate
        }

        public void Cursor(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            NodeCollectionMode mode = this.GetNodeCollectionMode(point, matrix);
            switch (mode)
            {
                case NodeCollectionMode.None:
                    CoreCursorExtension.IsPointerEntered = false;
                    CoreCursorExtension.None();
                    break;
                case NodeCollectionMode.Preview:
                    break;
                case NodeCollectionMode.Add:
                    break;
                case NodeCollectionMode.Move:
                    CoreCursorExtension.IsPointerEntered = true;
                    CoreCursorExtension.Cross();
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    CoreCursorExtension.IsPointerEntered = true;
                    CoreCursorExtension.Cross();
                    break;
                case NodeCollectionMode.RectChoose:
                    CoreCursorExtension.IsPointerEntered = false;
                    CoreCursorExtension.None();
                    break;
                default:
                    CoreCursorExtension.IsPointerEntered = false;
                    CoreCursorExtension.None();
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
                        drawingSession.DrawWireframe(layer2, matrix, this.ViewModel.AccentColor);
                        drawingSession.DrawNodeCollection(layer2.Nodes, matrix, this.ViewModel.AccentColor);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            drawingSession.DrawWireframe(layer, matrix, this.ViewModel.AccentColor);
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
                        // Snapping
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


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate(); // Invalidate
        }
        public void OnNavigatedFrom()
        {
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }
    }

    public partial class NodeTool : Page, ITool
    {

        // Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RemoveTextBlock.Text = resource.GetString("Tools_Node_Remove");
            this.InsertTextBlock.Text = resource.GetString("Tools_Node_Insert");
            this.SharpTextBlock.Text = resource.GetString("Tools_Node_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("Tools_Node_Smooth");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }


        private void Remove()
        {
            IList<Layerage> removeLayerage = new List<Layerage>();


            {
                // History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_RemoveNodes);

                // Selection
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
                                        // Refactoring
                                        layer.IsRefactoringTransformer = true;
                                        layer.IsRefactoringRender = true;
                                        layer.IsRefactoringIconRender = true;
                                        layerage.RefactoringParentsTransformer();
                                        layerage.RefactoringParentsRender();
                                        layerage.RefactoringParentsIconRender();
                                        layer.Nodes.NodesReplace(previous);
                                    };

                                    // Refactoring
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

                // History
                this.ViewModel.HistoryPush(history);
            }


            // Remove
            if (removeLayerage.Count != 0)
            {
                // History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_RemoveLayers);
                this.ViewModel.HistoryPush(history);

                foreach (Layerage remove in removeLayerage)
                {
                    LayerManager.Remove(remove);
                }

                // Selection
                this.SelectionViewModel.SetMode(); // Selection
                LayerManager.ArrangeLayers();
            }

            this.ViewModel.Invalidate(); // Invalidate
        }

        private void Insert()
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_InsertNodes);

            // Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    var previous = layer.Nodes.NodesStartingClone().ToList();
                    history.UndoAction += () =>
                    {
                        // Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        layer.Nodes.NodesReplace(previous);
                    };

                    // Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsTransformer();
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    NodeCollection.InterpolationCheckedNodes(layer.Nodes);
                }
            });

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate
        }


        private void Sharp()
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_SharpNodes);

            // Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    // Refactoring
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
                        // History
                        var previous = layer.Nodes.NodesStartingClone().ToList();
                        history.UndoAction += () =>
                        {
                            // Refactoring
                            layer.IsRefactoringTransformer = true;
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Nodes.NodesReplace(previous);
                        };
                    }
                }
            });

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate
        }

        private void Smooth()
        {
            // History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_Set_SmoothNodes);

            // Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    // Refactoring
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
                        // History
                        var previous = layer.Nodes.NodesStartingClone().ToList();
                        history.UndoAction += () =>
                        {
                            // Refactoring
                            layer.IsRefactoringTransformer = true;
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Nodes.NodesReplace(previous);
                        };
                    }
                }
            });

            // History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate(); // Invalidate
        }

    }
}