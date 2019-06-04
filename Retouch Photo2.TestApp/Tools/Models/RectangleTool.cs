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
    /// <see cref="Tool"/>'s RectangleTool .
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
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();
            RectangleLayer layer = new RectangleLayer
            {
                Transformer = new Transformer
                (
                    Vector2.Transform(startingPoint, inverseMatrix),
                    Vector2.Transform(point, inverseMatrix)                    
                )
            };
            this.ViewModel.TurnOnMezzanine(layer);//Mezzanine

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();

            TransformerVectors vectors = new TransformerVectors
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix)
            );
            this.ViewModel.MezzanineLayer.Transformer.DestinationVectors = vectors;

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (isSingleStarted)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();
                RectangleLayer layer = new RectangleLayer
                {
                    Transformer = new Transformer
                    (
                        Vector2.Transform(startingPoint, inverseMatrix),
                        Vector2.Transform(point, inverseMatrix)
                    )
                };
                this.ViewModel.InsertMezzanine(layer);//Mezzanine
            }
            else this.ViewModel.TurnOffMezzanine();//Mezzanine

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}