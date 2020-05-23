using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
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
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Layerage CurveLayerage => this.ViewModel.Layerage;
        CurveLayer CurveLayer => this.ViewModel.CurveLayer;
        NodeCollection Nodes => this.CurveLayer.Nodes;

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
                //TODO
                return;
                if (this.CurveLayer == null) return;

                bool isSuccessful = NodeCollection.RemoveCheckedNodes(this.CurveLayer.Nodes);
                if (isSuccessful == false)
                {

               //     LayerCollection.RemoveLayer(this.ViewModel.LayerCollection, this.CurveLayer);

               this.ViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate
                }
            };
            this.InsertButton.Click += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.Interpolation(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };

            this.SharpButton.Click += (s, e) =>
            {
                if (this.CurveLayer == null) return;
                NodeCollection.SharpCheckedNodes(this.CurveLayer.Nodes);
                this.ViewModel.Invalidate();//Invalidate
            };
            this.SmoothButton.Click += (s, e) =>
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

            //The NodeTool may change the current CurveLayer's transformer.
            Transformer transformer = this.CurveLayer.GetActualDestinationWithRefactoringTransformer;
            this.ViewModel.Transformer = transformer;
        }

    }

    /// <summary>
    /// <see cref="ITool"/>'s NodeTool.
    /// </summary>
    public partial class NodeTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Pen");

            this.RemoveTextBlock.Text = resource.GetString("/Tools/Pen_Remove");
            this.InsertTextBlock.Text = resource.GetString("/Tools/Pen_Insert");
            this.SharpTextBlock.Text = resource.GetString("/Tools/Pen_Sharp");
            this.SmoothTextBlock.Text = resource.GetString("/Tools/Pen_Smooth");
        }


        //@Content
        public ToolType Type => ToolType.Node;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new NodeIcon();
        readonly ToolButton _button = new ToolButton(new NodeIcon());


        //Pen
        public NodeCollectionMode Mode = NodeCollectionMode.None;
        Node _oldNode;
        TransformerRect _transformerRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            if (this.CurveLayer == null)
                this.Mode = NodeCollectionMode.None;
            else
                this.Mode = NodeCollection.ContainsNodeCollectionMode(startingPoint, this.Nodes, matrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Move:
                    {
                        //Snap
                        if (this.IsSnap) this.ViewModel.VectorVectorSnapStarted(this.Nodes);

                        this.Nodes.CacheTransform(isOnlySelected: true);
                    }
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
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                        this._transformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.CurveLayer == null) return;


            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Move:
                    {
                        //Snap
                        if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

                        Vector2 vector = canvasPoint - canvasStartingPoint;
                        this.Nodes.TransformAdd(vector, isOnlySelected: true);
                        this.ViewModel.TextVisibility = Visibility.Visible;
                        this.ViewModel.Text = canvasPoint.X.ToString();
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

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.CurveLayer == null) return;

            if (isOutNodeDistance)
            {
                switch (this.Mode)
                {
                    case NodeCollectionMode.Move:
                        {
                            //Snap
                            if (this.IsSnap)
                            {
                                canvasPoint = this.Snap.Snap(canvasPoint);
                                this.Snap.Default();
                            }

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

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            if (this.CurveLayer == null)
            {
                this.TipViewModel.MoveTool.Clicke(point);
            }
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.CurveLayer == null) return;

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            drawingSession.DrawNodeCollection(this.Nodes, matrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.RectChoose:
                    {
                        CanvasGeometry canvasGeometry = this._transformerRect.ToRectangle(this.ViewModel.CanvasDevice);
                        CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                        drawingSession.DrawGeometryDodgerBlue(canvasGeometryTransform);
                    }
                    break;
            }

            //Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}