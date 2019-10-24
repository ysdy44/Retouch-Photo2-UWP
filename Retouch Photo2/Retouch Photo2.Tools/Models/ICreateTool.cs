using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;

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
        
        //@Abstract
        /// <summary>
        /// Create a specific layer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        public abstract ILayer CreateLayer(Transformer transformer);

        public abstract ToolType Type { get; }
        public abstract FrameworkElement Icon { get; }
        public abstract IToolButton Button { get; }
        public abstract IToolPage Page { get; }

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

            //Tip
            this.ViewModel.TipWidthHeight?.Invoke(transformer, point, InvalidateMode.HD); //Delegate

            //Mezzanine
            this.ViewModel.MezzanineLayer = this.CreateLayer(transformer);
            this.ViewModel.MezzanineLayer.StyleManager = this.SelectionViewModel.StyleManager.Clone();
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(this.ViewModel.MezzanineLayer);

            //Selection
            this.SelectionViewModel.Transformer = transformer;
            this.SelectionViewModel.DeliverBrushPoints(this.ViewModel.MezzanineLayer);

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

            //Tip
            this.ViewModel.TipWidthHeight?.Invoke(transformer, point, InvalidateMode.None); //Delegate

            //Mezzanine
            this.ViewModel.MezzanineLayer.TransformManager.Source = transformer;
            this.ViewModel.MezzanineLayer.TransformManager.Destination = transformer;

            //DeliverBrushPoints
            this.ViewModel.MezzanineLayer.StyleManager.FillBrush.DeliverBrushPoints(transformer);
            this.ViewModel.MezzanineLayer.StyleManager.StrokeBrush.DeliverBrushPoints(transformer);

            //Selection
            this.SelectionViewModel.Transformer = transformer;
            this.SelectionViewModel.DeliverBrushPoints(this.ViewModel.MezzanineLayer);

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

                //Tip
                this.ViewModel.TipWidthHeight?.Invoke(transformer, point, InvalidateMode.Thumbnail); //Delegate

                //Mezzanine
                this.ViewModel.MezzanineLayer.TransformManager.Source = transformer;
                this.ViewModel.MezzanineLayer.TransformManager.Destination = transformer;

                foreach (ILayer child in this.ViewModel.Layers.RootLayers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }
                this.ViewModel.MezzanineLayer.SelectMode = SelectMode.Selected;
                this.ViewModel.MezzanineLayer = null;

                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
            }
            else this.ViewModel.Layers.RemoveMezzanineLayer(this.ViewModel.MezzanineLayer);//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

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