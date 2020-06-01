using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
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
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
                this.Title = resource.GetString("/Tools/Pen");
        }


        //@Content
        public ToolType Type => ToolType.Pen;
        public string Title { get; set; }
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

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(startingPoint, inverseMatrix);

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
                        //Snap
                        if (this.IsSnap) this.ViewModel.VectorVectorSnapInitiate(this.CurveLayer.Nodes);

                        Node node = new Node
                        {
                            Point = canvasPoint,
                            LeftControlPoint = canvasPoint,
                            RightControlPoint = canvasPoint,
                            IsChecked = false,
                            IsSmooth = false,
                        };

                        this._addEndNode = this.CurveLayer.Nodes.Last(n => n.Type == NodeType.Node);
                        this._addLastNode = this.CurveLayer.Nodes.Last(n => n.Type == NodeType.Node);
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
                            //Snap
                            if (this.IsSnap) canvasPoint = this.Snap.Snap(canvasPoint);

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
            else if (this.Mode == NodeCollectionMode.Add)
            {
                //Snap
                if (this.IsSnap)
                {
                    canvasPoint = this.Snap.Snap(canvasPoint);
                    this.Snap.Default();
                }

                Node node = new Node
                {
                    Point = canvasPoint,
                    LeftControlPoint = canvasPoint,
                    RightControlPoint = canvasPoint,
                    IsChecked = false,
                    IsSmooth = false,
                };
                this.CurveLayer.Nodes.PenAdd(node);
            }

            //Refactoring
            this.CurveLayer.IsRefactoringTransformer = true;
            this.CurveLayer.IsRefactoringRender = true;
            this.CurveLayer.IsRefactoringIconRender = true;
            this.CurveLayerage.RefactoringParentsTransformer();
            this.CurveLayerage.RefactoringParentsRender();
            this.CurveLayerage.RefactoringParentsIconRender();

            this.Mode = NodeCollectionMode.None;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        public void Clicke(Vector2 point)
        {
            if (this.CurveLayer == null) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            
            if (this.Mode == NodeCollectionMode.Add)
            {
                Node node = new Node
                {
                    Point = canvasPoint,
                    LeftControlPoint = canvasPoint,
                    RightControlPoint = canvasPoint,
                    IsChecked = false,
                    IsSmooth = false,
                };
                this.CurveLayer.Nodes.PenAdd(node);
            }

            //Refactoring
            this.CurveLayer.IsRefactoringTransformer = true;
            this.Mode = NodeCollectionMode.None;

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
            drawingSession.DrawNodeCollection(this.CurveLayer.Nodes, matrix);

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

            //Snapping
            if (this.IsSnap)
            {
                this.Snap.Draw(drawingSession, matrix);
                this.Snap.DrawNode2(drawingSession, matrix);
            }
        }

    }
}
