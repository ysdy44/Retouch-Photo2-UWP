using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s ICreateTool.
    /// </summary>
    public abstract class ICreateTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        bool IsCenter => this.KeyboardViewModel.IsCenter;
        bool IsSquare => this.KeyboardViewModel.IsSquare;

        ILayer MezzanineLayer;

        //@Abstract
        /// <summary>
        /// Create a specific layer.
        /// </summary>
        /// <param name="layerCollection"> The layer-collection. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        public abstract ILayer CreateLayer(LayerCollection layerCollection, Transformer transformer);

        public abstract bool IsSelected { set; }
        public abstract ToolType Type { get; }
        public abstract FrameworkElement Icon { get; }
        public abstract IToolButton Button { get; }
        public abstract Page Page { get; }

        public void Starting(Vector2 point) { }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerTool.Started(startingPoint, point, isSetTransformerMode: true)) return;//TransformerTool

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.IsCenter,
                 this.IsSquare
            );

            //Mezzanine
            this.MezzanineLayer = this.CreateLayer(this.ViewModel.Layers, transformer);
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(this.MezzanineLayer);

            this.SelectionViewModel.Transformer = transformer;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerTool.Delta(startingPoint, point)) return;//TransformerTool

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.IsCenter,
                 this.IsSquare
            );

            //Mezzanine
            this.MezzanineLayer.TransformManager.Source = transformer;
            this.MezzanineLayer.TransformManager.Destination = transformer;

            this.SelectionViewModel.Transformer = transformer;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (this.TransformerTool.Complete(startingPoint, point, isSingleStarted)) return;//TransformerTool

            if (isSingleStarted)
            {
                //Transformer
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Transformer transformer = new Transformer
                (
                     Vector2.Transform(startingPoint, inverseMatrix),
                     Vector2.Transform(point, inverseMatrix),
                     this.IsCenter,
                     this.IsSquare
                );

                //Mezzanine
                this.MezzanineLayer.TransformManager.Source = transformer;
                this.MezzanineLayer.TransformManager.Destination = transformer;

                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
            }
            else this.ViewModel.Layers.RemoveMezzanineLayer(this.MezzanineLayer);//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers.RootLayers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            this.TransformerTool.Draw(drawingSession);//TransformerTool
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}