using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        GeometryCurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;
        NodeCollection Nodes => this.CurveLayer.Nodes;

        /// <summary> PenPage's Flyout. </summary>
        public PenModeControl PenFlyout => this._penFlyout;
        

        //@Construct
        public PenTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);          
            
            this.RemoveButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.RemoveCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.AddButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.Interpolation(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SharpCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Tapped += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SmoothCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
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

    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Pen");

            this.AddOrMoveOnOffSwitch.OnContent = resource.GetString("/Tools/Pen_AddOrMoveOn");
            this.AddOrMoveOnOffSwitch.OffContent = resource.GetString("/Tools/Pen_AddOrMoveOff");

            this.RemoveTextBlock.Text = resource.GetString("/Tools/Pen_Remove");
            this.InsertTextBlock.Text = resource.GetString("/Tools/Pen_Insert");
            this.SharpTextBlock.Text = resource.GetString("/Tools/Pen_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("/Tools/Pen_Smooth");
        }


        //@Content
        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new PenIcon();
        readonly ToolButton _button = new ToolButton(new PenIcon());


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
                        this.Nodes[this.Nodes.Index] = this.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        this.Nodes[this.Nodes.Index] = this.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
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
                            this.Nodes[this.Nodes.Index] = this.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                            break;
                        case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                            this.Nodes[this.Nodes.Index] = this.PenFlyout.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
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

    }

    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : ITool
    {
        Vector2 _previewLeft;
        Vector2 _previewRight;

        /// <summary> Only the left point. </summary>
        bool _hasPreviewTempLeftPoint;

        public void PreviewStart(Vector2 canvasPoint)
        {
            if (this._hasPreviewTempLeftPoint == false) this._previewLeft = canvasPoint;
            this._previewRight = canvasPoint;
        }
        public void PreviewDelta(Vector2 canvasPoint)
        {
            this._previewRight = canvasPoint;
        }
        public void PreviewComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint, bool isSingleStarted)
        {
            if (this._hasPreviewTempLeftPoint)
            {
                this._hasPreviewTempLeftPoint = false;
                this.CreateLayer(this.ViewModel.Layers, this._previewLeft, canvasPoint);
            }
            else if (isSingleStarted)
            {
                this._hasPreviewTempLeftPoint = false;
                this.CreateLayer(this.ViewModel.Layers, canvasStartingPoint, canvasPoint);
            }
            else
            {
                this._hasPreviewTempLeftPoint = true;
            }
        }

        /// <summary>
        /// Draw a line before creating a curve layer.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        public void PreviewDraw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Vector2 lineLeft = Vector2.Transform(this._previewLeft, matrix);

            if (this._hasPreviewTempLeftPoint)
            {
                drawingSession.DrawNode3(lineLeft);
            }
            else
            {
                Vector2 lineRight = Vector2.Transform(this._previewRight, matrix);

                drawingSession.DrawLineDodgerBlue(lineLeft, lineRight);
                drawingSession.DrawNode3(lineLeft);
                drawingSession.DrawNode3(lineRight);
            }
        }

        private void CreateLayer(LayerCollection layerCollection, Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            GeometryCurveLayer curveLayer = new GeometryCurveLayer(canvasStartingPoint, canvasPoint)
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
            };

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.SelectMode = SelectMode.UnSelected;
            });

            //Mezzanine
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(curveLayer);
            this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}