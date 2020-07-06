using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;

        /// <summary> PenPage's Flyout. </summary>
        public PenModeControl PenFlyout => this._PenFlyout;


        //@Construct
        /// <summary>
        /// Initializes a NodeTool. 
        /// </summary>
        public NodeTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.MoreButton.Click += (s, e) => this.Flyout.ShowAt(this);

            this.RemoveButton.Click += (s, e) =>
            {
                IList<Layerage> removeLayerage = new List<Layerage>();


                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Remove nodes");

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
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory("Remove layers", this.ViewModel.LayerageCollection);
                    this.ViewModel.HistoryPush(history);

                    foreach (Layerage remove in removeLayerage)
                    {
                        LayerageCollection.Remove(this.ViewModel.LayerageCollection, remove);
                    }

                    //Selection
                    this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                }

                this.ViewModel.Invalidate();//Invalidate
            };
            this.InsertButton.Click += (s, e) =>
            {                 
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Insert nodes");
                
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

            this.SharpButton.Click += (s, e) =>
            {         
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Sharp nodes");
                
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
                LayersPropertyHistory history = new LayersPropertyHistory("Smooth nodes");

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
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }
    }

    /// <summary>
    /// <see cref="ITool"/>'s NodeTool.
    /// </summary>
    public partial class NodeTool : Page, ITool
    {

        private Layerage GetNodeCollectionLayer(Vector2 startingPoint, Matrix3x2 matrix)
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;

                case ListViewSelectionMode.Single:
                    {
                        Layerage layerage = this.SelectionViewModel.SelectionLayerage;
                        if (layerage == null) return null;
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            layer.Nodes.CacheTransform();
                            this.NodeCollectionMode = NodeCollection.ContainsNodeCollectionMode(startingPoint, layer.Nodes, matrix);
                            return layerage;
                        }
                    }
                    break;

                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.SelectionViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;

                        if (layer.Type == LayerType.Curve)
                        {
                            layer.Nodes.CacheTransform();
                            NodeCollectionMode mode = NodeCollection.ContainsNodeCollectionMode(startingPoint, layer.Nodes, matrix);
                            if (mode != NodeCollectionMode.None)
                            {
                                this.NodeCollectionMode = mode;
                                return layerage;
                            }
                        }
                    }
                    break;
            }
            return null;
        }

    }
}