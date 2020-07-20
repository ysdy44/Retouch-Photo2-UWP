using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s NodeTool.
    /// </summary>
    public partial class NodeTool : ITool
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


        private void MoveStarted()
        {
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
        private void MoveDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
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
        private void MoveComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Nodes.NodesReplace(previous);
                    };

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


        private void MoveSingleNodePointStarted(Vector2 startingPoint, Matrix3x2 matrix)
        {
            //Selection
            ILayer layer = this.Layerage.Self;

            if (layer.Type == LayerType.Curve)
            {
                layer.Nodes.SelectionOnlyOne(startingPoint, matrix);

                //Snap
                if (this.IsSnap) this.ViewModel.VectorVectorSnapInitiate(layer.Nodes);
            }
        }
        private void MoveSingleNodePointDelta(Vector2 canvasPoint)
        {
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
        private void MoveSingleNodePointComplete(Vector2 canvasPoint)
        {
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
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Nodes[previous] = previous1;
                };

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


        private void MoveSingleNodeControlPointStarted()
        {
            //Selection
            ILayer layer = this.Layerage.Self;

            if (layer.Type == LayerType.Curve)
            {
                layer.Nodes.SelectedItem.CacheTransform();
            }
        }
        private void MoveSingleNodeControlPointDelta(Vector2 canvasPoint, bool isLeftControlPoint)
        {
            ILayer layer = this.Layerage.Self;

            if (layer.Type == LayerType.Curve)
            {
                Node node = layer.Nodes.SelectedItem;

                //Refactoring
                layer.IsRefactoringRender = true;
                this.Layerage.RefactoringParentsRender();
                Node.Controller(this.NodePage.PenFlyout.SelfMode, this.NodePage.PenFlyout.EachLengthMode, this.NodePage.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint);
            }
        }
        private void MoveSingleNodeControlPointComplete(Vector2 canvasPoint, bool isLeftControlPoint)
        {
            ILayer layer = this.Layerage.Self;

            if (layer.Type == LayerType.Curve)
            {
                Node node = layer.Nodes.SelectedItem;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Move node control point");

                var previous = layer.Nodes.Index;
                var previous1 = node.StartingLeftControlPoint;
                var previous2 = node.StartingRightControlPoint;
                history.UndoAction += () =>
                {
                    Node node2 = layer.Nodes[previous];

                    //Refactoring
                    layer.IsRefactoringTransformer = true;
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    node2.LeftControlPoint = previous1;
                    node2.RightControlPoint = previous2;
                };

                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                this.Layerage.RefactoringParentsTransformer();
                this.Layerage.RefactoringParentsRender();
                this.Layerage.RefactoringParentsIconRender();
                Node.Controller(this.NodePage.PenFlyout.SelfMode, this.NodePage.PenFlyout.EachLengthMode, this.NodePage.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint);

                //History
                this.ViewModel.HistoryPush(history);
            }
        }


        private void RectChooseStarted(Vector2 startingPoint, Vector2 point)
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
        private void RectChooseDelta(Vector2 canvasStartingPoint, Vector2 canvasPoint)
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
        private void RectChooseComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint)
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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringTransformer = true;
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Nodes.NodesReplace(previous);
                    };
                }
            });

            //History
            this.ViewModel.HistoryPush(history);
        }


    }
}