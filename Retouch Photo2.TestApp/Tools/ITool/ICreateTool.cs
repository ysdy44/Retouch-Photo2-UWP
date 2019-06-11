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
                Vector2.Transform(point, inverseMatrix)
            );

            //Mezzanine
            this.ViewModel.Mezzanine.SetLayer(this.CreateLayer(transformer),this.ViewModel.Layers);
            
            this.ViewModel.SelectionTransformer = transformer;//Selection

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
                 Vector2.Transform(point, inverseMatrix)
            );

            this.ViewModel.Mezzanine.Layer.TransformerMatrix.Destination = transformer;//Mezzanine

            this.ViewModel.SelectionTransformer = transformer;//Selection

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
                    Vector2.Transform(point, inverseMatrix)
                );
                
                //Selection
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    layer.IsChecked = false;
                });
                this.ViewModel.Mezzanine.Insert(this.CreateLayer(transformer), this.ViewModel.Layers); //Mezzanine
            }
            else this.ViewModel.Mezzanine.None();//Mezzanine

            this.ViewModel.SetSelectionMode();//Selection

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}