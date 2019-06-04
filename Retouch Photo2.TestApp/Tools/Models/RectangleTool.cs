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
            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();
            TransformerVectors transformerVectors = new TransformerVectors
            (
                Vector2.Transform(startingPoint, inverseMatrix),
                Vector2.Transform(point, inverseMatrix)
            );

            //Layer
            RectangleLayer rectangleLayer = new RectangleLayer
            {
                Transformer = new Transformer(transformerVectors)
            };
            this.ViewModel.TurnOnMezzanine(rectangleLayer);//Mezzanine

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {         
            //Transformer
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();
            TransformerVectors transformerVectors = new TransformerVectors
            (
                 Vector2.Transform(startingPoint, inverseMatrix),
                 Vector2.Transform(point, inverseMatrix)
            );

            //Transformer
            this.ViewModel.TransformerVectors = transformerVectors;

            //Layer
            this.ViewModel.MezzanineLayer.Transformer.DestinationVectors = transformerVectors;

            this.ViewModel.Invalidate();//Invalidate
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            if (isSingleStarted)
            {
                //Transformer
                Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.GetInverseMatrix();
                TransformerVectors transformerVectors = new TransformerVectors
                (
                    Vector2.Transform(startingPoint, inverseMatrix),
                    Vector2.Transform(point, inverseMatrix)
                );

                //Transformer
                this.ViewModel.LayerUnChecked();
                this.ViewModel.TransformerVectors = transformerVectors;

                //Layer
                RectangleLayer rectangleLayer = new RectangleLayer
                {
                    IsChecked = true,
                    Transformer = new Transformer(transformerVectors)
                };
                this.ViewModel.InsertMezzanine(rectangleLayer);//Mezzanine
            }
            else
            {
                //Transformer
                this.ViewModel.TransformerVectors = this.ViewModel.GetCheckedLayersTransformerVectors(this.ViewModel.Layers);

                //Layer
                this.ViewModel.TurnOffMezzanine();//Mezzanine
            }

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }

        public override void Draw(CanvasDrawingSession ds) { }
    }
}