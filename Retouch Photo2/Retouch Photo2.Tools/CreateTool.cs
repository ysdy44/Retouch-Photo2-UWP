using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s ICreateTool.
    /// </summary>
    public class CreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        bool IsCenter => this.KeyboardViewModel.IsCenter;
        bool IsSquare => this.KeyboardViewModel.IsSquare;

        /// <summary>
        /// Function of how to crate a layer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        public Func<Transformer, ILayer> CreateLayer;


        //@Construct      
        /// <summary>
        /// Construct a  <see cref="CreateTool"/>
        /// </summary>
        public CreateTool() { }
        /// <summary>
        /// Construct a  <see cref="CreateTool"/>
        /// </summary>
        /// <param name="createLayer"> Create a specific layer. </param>
        public CreateTool(Func<Transformer, ILayer> createLayer) => this.CreateLayer = createLayer;



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

            //Text
            this.ViewModel.SetTextWidthHeight(transformer);
            this.ViewModel.TextVisibility = Visibility.Visible;

            //Mezzanine
            ILayer layer = this.CreateLayer(transformer);
            layer.StyleManager = this.SelectionViewModel.StyleManager.Clone();

            this.ViewModel.MezzanineLayer = layer;
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

            //Text
            this.ViewModel.SetTextWidthHeight(transformer);

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

                //Text
                this.ViewModel.TextVisibility = Visibility.Collapsed;

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
    }
}