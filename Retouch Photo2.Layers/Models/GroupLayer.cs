// Core:              ★★★★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★★
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Group;


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            // Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                if (layerage.Children.Count != 0)
                {
                    // TransformerBorder
                    TransformerBorder border = new TransformerBorder(layerage.Children);
                    Transformer transformer = border.ToTransformer();
                    this.Transform.Transformer = transformer;
                    return transformer;
                }
            }

            return this.Transform.GetActualTransformer();
        }

        public override ILayer Clone() => LayerBase.CopyWith(this, new GroupLayer());
        

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            if (layerage.Children.Count == 0) return null;

            ICanvasImage childImage = LayerBase.Render(resourceCreator, layerage);
            if (childImage == null) return null;

            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                if (this.Transform.IsCrop)
                {
                    CanvasGeometry geometryCrop = this.Transform.CropTransformer.ToRectangle(resourceCreator);

                    using (drawingSession.CreateLayer(1, geometryCrop))
                    {
                        drawingSession.DrawImage(childImage);
                    }
                }
                else
                {
                    drawingSession.DrawImage(childImage);
                }
            }
            return command;
        }


        public override void DrawWireframe(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            Transformer transformer = base.Transform.GetActualTransformer();
            drawingSession.DrawBound(transformer, matrix);
        }


        public override bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;

            foreach (Layerage layerage2 in layerage.Children)
            {
                ILayer layer = layerage2.Self;

                if (layer.FillContainsPoint(layerage2, point))
                {
                    return true;
                }
            }

            return false;
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;

    }
}