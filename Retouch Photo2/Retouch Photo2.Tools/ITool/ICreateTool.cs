using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;

namespace Retouch_Photo2.Tools.ITool
{
    /// <summary>
    /// <see cref="Tool"/>'s ICreateTool.
    /// </summary>
    public abstract class ICreateTool : Tool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Abstract
        /// <summary>
        /// Create a specific layer.
        /// </summary>
        /// <param name="transformer"> transformer </param>
        /// <returns> Layer </returns>
        public abstract Layer CreateLayer(Transformer transformer);

        //@Construct
        public ICreateTool()
        {
        }

        //@Override
        public override void Starting(Vector2 point) { }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            if (this.TipViewModel.TransformerTool.Started(startingPoint)) return;//TransformerToolBase

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.KeyboardViewModel.IsCenter,
                 this.KeyboardViewModel.IsRatio
            );

            //Mezzanine
            this.MezzanineViewModel.SetLayer(this.CreateLayer(transformer),this.ViewModel.Layers);

            this.SelectionViewModel.Transformer = transformer;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            if (this.TipViewModel.TransformerTool.Delta(startingPoint, point)) return;//TransformerToolBase

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.KeyboardViewModel.IsCenter,
                 this.KeyboardViewModel.IsRatio
            );

            this.MezzanineViewModel.Layer.Source = transformer;//Mezzanine
            this.MezzanineViewModel.Layer.Destination = transformer;//Mezzanine

            this.SelectionViewModel.Transformer = transformer;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (this.TipViewModel.TransformerTool.Complete(isSingleStarted)) return;//TransformerToolBase

            if (isSingleStarted)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Transformer transformer = new Transformer
                (
                     Vector2.Transform(startingPoint, inverseMatrix),
                     Vector2.Transform(point, inverseMatrix),
                     this.KeyboardViewModel.IsCenter,
                     this.KeyboardViewModel.IsRatio
                );

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });
                this.MezzanineViewModel.Insert(this.CreateLayer(transformer), this.ViewModel.Layers); //Mezzanine
            }
            else this.MezzanineViewModel.None();//Mezzanine

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            this.TipViewModel.TransformerTool.Draw(ds);//TransformerToolBase
        }
    }
}