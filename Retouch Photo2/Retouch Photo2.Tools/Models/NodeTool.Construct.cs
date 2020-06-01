using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
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
                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                layer.Nodes.CacheTransformOnlySelected();
                            }
                        });
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                layer.Nodes.SelectionOnlyOne(startingPoint, matrix);
                            }
                        });
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                        this.TransformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                    }
                    break;
            }

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.Layerage == null) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.NodeCollectionMode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Move:
                    {
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
                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layerage.RefactoringParentsRender();
                                Node node = layer.Nodes.SelectedItem;
                                Node.Move(point, node);
                            }
                        });
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layerage.RefactoringParentsRender();
                                Node node = layer.Nodes.SelectedItem;
                                Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: true);
                            }
                        });
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layerage) =>
                        {
                            ILayer layer = layerage.Self;

                            if (layer.Type == LayerType.Curve)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layerage.RefactoringParentsRender();
                                Node node = layer.Nodes.SelectedItem;
                                Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: false);
                            }
                        });
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

            if (this.Layerage == null) return;

            if (isOutNodeDistance)
            {
                switch (this.NodeCollectionMode)
                {
                    case NodeCollectionMode.Move:
                        {
                            Vector2 canvasMove = canvasPoint - canvasStartingPoint;

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
                                    layer.Nodes.TransformAddOnlySelected(canvasMove);
                                }
                            });
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        {
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
                                    Node node = layer.Nodes.SelectedItem;
                                    Node.Move(canvasPoint, node);
                                }
                            });
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        {
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
                                    Node node = layer.Nodes.SelectedItem;
                                    Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: true);
                                }
                            });
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        {
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
                                    Node node = layer.Nodes.SelectedItem;
                                    Node.Controller(this.PenFlyout.SelfMode, this.PenFlyout.EachLengthMode, this.PenFlyout.EachAngleMode, canvasPoint, node, isLeftControlPoint: false);
                                }
                            });
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
            }


            this.NodeCollectionMode = NodeCollectionMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                if (layer.Type == LayerType.Curve)
                {
                    layer.Nodes.SelectionOnlyOne(point, matrix);
                }
            });

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
                case NodeCollectionMode.RectChoose:
                    {
                        CanvasGeometry canvasGeometry = this.TransformerRect.ToRectangle(this.ViewModel.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }

            //Snapping
            //  if (this.IsSnap)
            //   {
            //       this.Snap.Draw(drawingSession, matrix);
            //       this.Snap.DrawNode2(drawingSession, matrix);
            //   }
        }

    }
}