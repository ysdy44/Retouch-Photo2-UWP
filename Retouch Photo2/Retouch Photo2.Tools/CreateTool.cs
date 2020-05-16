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
    /// <see cref="ICreateTool"/>'s CreateTool.
    /// </summary>
    public class CreateTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        bool IsCenter => this.SettingViewModel.IsCenter;
        bool IsSquare => this.SettingViewModel.IsSquare;


        /// <summary>
        /// Occurs when the operation begins. 
        /// </summary>
        /// <param name="createLayer">
        /// <summary>
        /// Function of how to crate a layer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        /// </param>
        /// <param name="startingPoint"> The starting pointer. </param>
        /// <param name="point"> The pointer. </param>
        public void Started(Func<Transformer, ILayer> createLayer, Vector2 startingPoint, Vector2 point)
        {
            if (this.TransformerTool.Started(startingPoint, point)) return;//TransformerTool

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
            this.ViewModel.MezzanineLayer = createLayer(transformer);
            this.ViewModel.MezzanineLayer.Style.CacheTransform();
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(this.ViewModel.MezzanineLayer);

            //Selection
            this.SelectionViewModel.Transformer = transformer;

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
            this.ViewModel.MezzanineLayer.Transform = new Transform(transformer);

            //TransformBrush
            this.ViewModel.MezzanineLayer.Style.DeliverBrushPoints(transformer);

            //Selection
            this.SelectionViewModel.Transformer = transformer;

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            if (this.TransformerTool.Complete(startingPoint, point)) return;//TransformerTool

            if (isOutNodeDistance)
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
                this.ViewModel.MezzanineLayer.Transform = new Transform(transformer); 

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