using FanKit.Transformers;
using FanKit.Transformers.Extensions;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Models.PenTools;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public class PenTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        NodeCollection NodeCollection => this.SelectionViewModel.CurveLayer.NodeCollection;
 
        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenControl();
        public FrameworkElement ShowIcon { get; } = new PenControl();
        public Page Page => this._penPage;
        PenPage _penPage { get; } = new PenPage();
        
        //Pen
        public NodeCollectionMode Mode = NodeCollectionMode.None;
        public readonly PreviewTool PreviewTool = new PreviewTool();
        public readonly AddTool AddTool = new AddTool();
        Node _oldNode;
        TransformerRect _transformerRect;

        public void Starting(Vector2 point)
        {
            if (this.SelectionViewModel.CurveLayer == null)
                this.Mode = NodeCollectionMode.Preview;
            else if (this.SelectionViewModel.IsPenToolNodesMode == false)
                this.Mode = NodeCollectionMode.Add;
            else
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                this.Mode = NodeCollection.ContainsNodeCollectionMode(point, this.NodeCollection, matrix);
            }

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewTool.Start(point);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    this.AddTool.Start(point);//AddNode
                    break;
                case NodeCollectionMode.Move:
                    this.NodeCollection.CacheTransform(isOnlySelected:true);
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.NodeCollection.SelectionOnlyOne(this.NodeCollection.Index);
                    this._oldNode = this.NodeCollection[this.NodeCollection.Index];
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    this._oldNode = this.NodeCollection[this.NodeCollection.Index];
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this._oldNode = this.NodeCollection[this.NodeCollection.Index];
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

                        this._transformerRect = new TransformerRect(canvasPoint, canvasPoint);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Started(Vector2 startingPoint, Vector2 point) { }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewTool.Delta(point);//PreviewNode
                }
                return;
            }

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    break;
                case NodeCollectionMode.Add:
                    this.AddTool.Delta(point);//AddNode
                    break;
                case NodeCollectionMode.Move:
                    {
                        Vector2 vector = canvasPoint - canvasStartingPoint;
                        this.NodeCollection.TransformAdd(vector);
                    }
                    break;
                case NodeCollectionMode.MoveSingleNodePoint:
                    this.NodeCollection[this.NodeCollection.Index] = this._oldNode.Move(canvasPoint);
                    break;
                case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                    this.NodeCollection[this.NodeCollection.Index] = this._penPage.Touchbar.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                    break;
                case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                    this.NodeCollection[this.NodeCollection.Index] = this._penPage.Touchbar.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        TransformerRect transformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                        this._transformerRect = transformerRect;
                        this.NodeCollection.RectChoose(transformerRect);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        { 
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewTool.Complete(startingPoint, point, isSingleStarted);//PreviewNode
                }
                return;
            }

            if (this.Mode == NodeCollectionMode.Add && this.SelectionViewModel.CurveLayer != null)
            {
                this.AddTool.Complete(point);//AddNode
                return;
            }

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (isSingleStarted)
            {
                switch (this.Mode)
                {
                    case NodeCollectionMode.Move:
                        {
                            Vector2 vector = canvasPoint - canvasStartingPoint;
                            this.NodeCollection.TransformAdd(vector);
                        }
                        break;
                    case NodeCollectionMode.MoveSingleNodePoint:
                        this.NodeCollection[this.NodeCollection.Index] = this._oldNode.Move(canvasPoint);
                        break;
                    case NodeCollectionMode.MoveSingleNodeLeftControlPoint:
                        this.NodeCollection[this.NodeCollection.Index] = this._penPage.Touchbar.Controller(canvasPoint, this._oldNode, isLeftControlPoint: true);
                        break;
                    case NodeCollectionMode.MoveSingleNodeRightControlPoint:
                        this.NodeCollection[this.NodeCollection.Index] = this._penPage.Touchbar.Controller(canvasPoint, this._oldNode, isLeftControlPoint: false);
                        break;
                    case NodeCollectionMode.RectChoose:
                        this._transformerRect = new TransformerRect(canvasStartingPoint, canvasPoint);
                        break;
                }
            }

            this.SelectionViewModel.CurveLayer.CorrectionTransformer();
            this.Mode = NodeCollectionMode.None;

            this.ViewModel.Invalidate();//Invalidate
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.SelectionViewModel.CurveLayer == null)
            {
                if (this.Mode == NodeCollectionMode.Preview)
                {
                    this.PreviewTool.Draw(drawingSession);//PreviewNode
                }
                return;
            }

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            switch (this.Mode)
            {
                case NodeCollectionMode.Add:
                    this.AddTool.Draw(drawingSession);//AddNode
                    break;
                case NodeCollectionMode.RectChoose:
                    {
                        TransformerRect transformerRect = this._transformerRect;
                        drawingSession.FillRectDodgerBlue(this.ViewModel.CanvasDevice, transformerRect, matrix);
                    }
                    break;
            }

            drawingSession.DrawNodeCollection(this.NodeCollection, matrix);
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            if (this.SelectionViewModel.CurveLayer == null) return;

            //The PenTool may change the current CurveLayer's transformer.
            this.SelectionViewModel.Transformer = this.SelectionViewModel.CurveLayer.Destination;
        }
    }
}