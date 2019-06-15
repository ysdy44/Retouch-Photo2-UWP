using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;
using System.Numerics;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s ICreateTool.
    /// </summary>
    public abstract class ICreateTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel Selection => Retouch_Photo2.TestApp.App.Selection;
        KeyboardViewModel Keyboard => Retouch_Photo2.TestApp.App.Keyboard;
        MezzanineViewModel Mezzanine => Retouch_Photo2.TestApp.App.Mezzanine;

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
            //Cursor
            if (this.ViewModel.CursorTool.CursorStarted(startingPoint)) return;

            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.Keyboard.IsCenter,
                 this.Keyboard.IsRatio
            );

            //Mezzanine
            this.Mezzanine.SetLayer(this.CreateLayer(transformer),this.ViewModel.Layers);
            
            this.Selection.Transformer = transformer;//Selection

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Cursor
            if (this.ViewModel.CursorTool.CursorDelta(startingPoint, point)) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Transformer transformer = new Transformer
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix),
                 this.Keyboard.IsCenter,
                 this.Keyboard.IsRatio
            );

            this.Mezzanine.Layer.TransformerMatrix.Destination = transformer;//Mezzanine

            this.Selection.Transformer = transformer;//Selection

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Cursor
            if (this.ViewModel.CursorTool.CursorComplete(isSingleStarted)) return;

            if (isSingleStarted)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Transformer transformer = new Transformer
                (
                     Vector2.Transform(startingPoint, inverseMatrix),
                     Vector2.Transform(point, inverseMatrix),
                     this.Keyboard.IsCenter,
                     this.Keyboard.IsRatio
                );

                //Selection
                this.Selection.SetValue((layer) =>
                {
                    layer.IsChecked = false;
                });
                this.Mezzanine.Insert(this.CreateLayer(transformer), this.ViewModel.Layers); //Mezzanine
            }
            else this.Mezzanine.None();//Mezzanine

            this.Selection.SetMode(this.ViewModel.Layers);//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}