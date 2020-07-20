using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s PenTool.
    /// </summary>
    public partial class PenTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;

        Layerage CurveLayerage => this.SelectionViewModel.CurveLayerage;
        CurveLayer CurveLayer => this.SelectionViewModel.CurveLayer;

        VectorVectorSnap Snap => this.ViewModel.VectorVectorSnap;
        bool IsSnap => this.SettingViewModel.IsSnap;


        //@Content
        public ToolType Type => ToolType.Pen;
        public FrameworkElement Icon { get; } = new PenIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new PenIcon()
        };
        public FrameworkElement Page { get; } = new GeometryPage();


        //@Construct
        /// <summary>
        /// Initializes a PenTool. 
        /// </summary>
        public PenTool()
        {
            this.ConstructStrings();
        }
        

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


        private void CreateLayer(Vector2 canvasStartingPoint, Vector2 canvasPoint)
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer", this.ViewModel.LayerageCollection);
            this.ViewModel.HistoryPush(history);


            //Transformer
            Transformer transformer = new Transformer(canvasPoint, canvasStartingPoint);

            //Layer
            CurveLayer curveLayer = new CurveLayer(this.ViewModel.CanvasDevice, canvasStartingPoint, canvasPoint)
            {
                IsSelected = true,
                Transform = new Transform(transformer),
                Style = this.SelectionViewModel.StandCurveStyle,
            };
            Layerage curveLayerage = curveLayer.ToLayerage();
            LayerBase.Instances.Add(curveLayer);

            //Mezzanine
            LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, curveLayerage);


            this.SelectionViewModel.SetModeSingle(curveLayerage);//Selection
            LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            //Refactoring
            this.SelectionViewModel.Transformer = this.SelectionViewModel.RefactoringTransformer();
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Pen");
        }

    }
}