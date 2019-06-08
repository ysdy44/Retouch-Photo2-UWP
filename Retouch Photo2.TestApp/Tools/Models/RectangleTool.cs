using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.ViewModels;
using System.Numerics;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s RectangleTool.
    /// </summary>
    public class RectangleTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        public RectangleTool()
        {
            base.Type = ToolType.Rectangle;
            base.Icon = new RectangleControl();
            base.ShowIcon = new RectangleControl();
            base.Page = null;
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
            this.ViewModel.TurnOnMezzanine(new RectangleLayer
            {
                TransformerMatrix = new TransformerMatrix(transformer)
            });

            this.ViewModel.Transformer=transformer;//Transformer

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
            
            this.ViewModel.MezzanineLayer.TransformerMatrix.Destination = transformer;//Mezzanine

            this.ViewModel.Transformer=transformer;//Transformer

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Cursor
            if (this.ViewModel.CursorTool.CursorComplete()) return;

            if (isSingleStarted)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Transformer transformer = new Transformer
                (
                    Vector2.Transform(startingPoint, inverseMatrix),
                    Vector2.Transform(point, inverseMatrix)
                );

                this.ViewModel.LayerAllUnChecked();//Layer

                //Mezzanine
                this.ViewModel.InsertMezzanine(new RectangleLayer
                {
                    IsChecked = true,
                    TransformerMatrix = new TransformerMatrix(transformer)
                });
            }
            else this.ViewModel.TurnOffMezzanine();//Mezzanine

            this.ViewModel.SetSelectionMode(this.ViewModel.Layers);//Transformer

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}