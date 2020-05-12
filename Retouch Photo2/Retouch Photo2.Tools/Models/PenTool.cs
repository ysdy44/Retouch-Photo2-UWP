using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        GeometryTool GeometryTool = new GeometryTool();

        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;
        NodeCollection Nodes => this.CurveLayer.Nodes;

        //@Construct
        public PenTool()
        {
            this.Content = this.GeometryTool;
            this.ConstructStrings();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.GeometryTool.OnNavigatedFrom();
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
            else 
                this.Mode = NodeCollectionMode.Add;

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
                }
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (this.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewComplete(canvasStartingPoint, canvasPoint, isOutNodeDistance);//PreviewNode
                }
            }
            else    if (this.Mode == NodeCollectionMode.Add)
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

                this.CurveLayer.IsRefactoringTransformer = true;//RefactoringTransformer
                this.Mode = NodeCollectionMode.None;

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Clicke(Vector2 point)
        {
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

            drawingSession.DrawNodeCollection(this.Nodes, matrix);

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
            }
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
        public void PreviewComplete(Vector2 canvasStartingPoint, Vector2 canvasPoint, bool isOutNodeDistance)
        {
            if (this._hasPreviewTempLeftPoint)
            {
                this._hasPreviewTempLeftPoint = false;
                this.CreateLayer(this.ViewModel.Layers, this._previewLeft, canvasPoint);
            }
            else if (isOutNodeDistance)
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
            CurveLayer curveLayer = new CurveLayer(canvasStartingPoint, canvasPoint)
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManagerCurve(),
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