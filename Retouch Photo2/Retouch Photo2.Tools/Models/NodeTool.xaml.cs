using FanKit.Transformers;
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
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        ListViewSelectionMode SelectionMode => this.SelectionViewModel.SelectionMode;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;

        /// <summary> PenPage's Flyout. </summary>
        public PenModeControl PenFlyout => this._penFlyout;


        //@Construct
        public NodeTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.MoreButton.Click += (s, e) => this.Flyout.ShowAt(this);

            this.RemoveButton.Click += (s, e) =>
            {
                IList<Layerage> removeLayerage = new List<Layerage>();

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer2 = layerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        NodeBorderCollection nodeBorderCollection = new NodeBorderCollection(layer2.Nodes);
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
                                    //Refactoring
                                    layer2.IsRefactoringTransformer = true;
                                    layer2.IsRefactoringRender = true;
                                    layer2.IsRefactoringIconRender = true;
                                    layerage.RefactoringParentsTransformer();
                                    layerage.RefactoringParentsRender();
                                    layerage.RefactoringParentsIconRender();
                                    IEnumerable<Node> uncheckedNodes = nodeBorderCollection.GetUnCheckedNodes();
                                    layer2.Nodes.NodesReplace(uncheckedNodes);
                                }
                                break;

                            default:
                                break;
                        }
                    }
                });


                //Remove
                if (removeLayerage.Count != 0)
                {
                    foreach (Layerage remove in removeLayerage)
                    {
                        LayerageCollection.RemoveLayer(this.ViewModel.LayerageCollection, remove);
                    }

                    //Selection
                    this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                }

                this.ViewModel.Invalidate();//Invalidate
            };
            this.InsertButton.Click += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer2 = layerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer2.IsRefactoringTransformer = true;
                        layer2.IsRefactoringRender = true;
                        layer2.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        NodeCollection.InterpolationCheckedNodes(layer2.Nodes);
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Click += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer2 = layerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer2.IsRefactoringTransformer = true;
                        layer2.IsRefactoringRender = true;
                        layer2.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        NodeCollection.SharpCheckedNodes(layer2.Nodes);
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Click += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer2 = layerage.Self;

                    if (layer2.Type == LayerType.Curve)
                    {
                        //Refactoring
                        layer2.IsRefactoringTransformer = true;
                        layer2.IsRefactoringRender = true;
                        layer2.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsTransformer();
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        NodeCollection.SmoothCheckedNodes(layer2.Nodes);
                    }
                });

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