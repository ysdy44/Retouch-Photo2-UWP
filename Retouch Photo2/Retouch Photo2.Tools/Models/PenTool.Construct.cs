using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
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

            this.Button.Title = resource.GetString("/Tools/Pen");
        }


        //@Content
        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new PenIcon()
        };
        public FrameworkElement Page => this;
        

        //Pen
        NodeCollectionMode Mode = NodeCollectionMode.None;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.CurveLayer == null)
                this.Mode = NodeCollectionMode.Preview;
            else
                this.Mode = NodeCollectionMode.Add;

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewStart(startingPoint);
                    break;
                case NodeCollectionMode.Add:
                    this.AddStart();
                    break;
            }
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewDelta(canvasPoint);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDelta(canvasPoint);
                        }
                    }
                    break;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.None:
                    break;
                case NodeCollectionMode.Preview:
                    this.PreviewComplete(canvasStartingPoint, canvasPoint, isOutNodeDistance);//PreviewNode
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddComplete(canvasPoint);
                        }
                    }
                    break;
            }

            this.Mode = NodeCollectionMode.None;
        }
        public void Clicke(Vector2 point)
        {
            if (this.CurveLayer == null) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Mode)
            {
                case NodeCollectionMode.Add:
                    this.AddComplete(canvasPoint);
                    break;
            }
        }


        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.Mode)
            {
                case NodeCollectionMode.Preview:
                    this.PreviewDraw(drawingSession);
                    break;
                case NodeCollectionMode.Add:
                    {
                        if (this.CurveLayer != null)
                        {
                            this.AddDraw(drawingSession);
                        }
                    }
                    break;
                default:
                    {
                        if (this.CurveLayer != null)
                        {
                            ILayer layer = this.CurveLayer;

                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

                            drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                            drawingSession.DrawNodeCollection(layer.Nodes, matrix, this.ViewModel.AccentColor);
                        }
                    }
                    break;
            }
        }

    }
}
