using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.Group;

        //@Construct
        /// <summary>
        /// Initializes a group-layer.
        /// </summary>
        public GroupLayer()
        {
            base.Control = new LayerControl
            {
                Icon = new GroupIcon(),
                Type = this.ConstructStrings(),
            };
        }


        public override Transformer GetActualDestinationWithRefactoringTransformer(Layerage layerage)
        {
            if (this.IsRefactoringTransformer)
            {
                //TransformerBorder
                TransformerBorder border = new TransformerBorder(layerage.Children);
                Transformer transformer = border.ToTransformer();

                this.Transform.Source = transformer;
                this.Transform.Destination = transformer;

                this.IsRefactoringTransformer = false;
            }

            return this.Transform.IsCrop ? this.Transform.CropDestination : this.Transform.Destination;
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GroupLayer groupLayer = new GroupLayer();

            Layer.CopyWith(resourceCreator, groupLayer, this);
            return groupLayer;
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix, IList<Layerage> children)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                foreach (Layerage child in children)
                {
                    ILayer child2 = child.Self; 

                    if (child2.Visibility == Visibility.Collapsed) continue;
                    if (child2.Opacity == 0) continue;

                    //GetRender
                    ICanvasImage currentImage = child2.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix, child.Children);
                    drawingSession.DrawImage(currentImage);
                }
            }
            return command;
        }
        public override void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, IList<Layerage> children, Windows.UI.Color accentColor)
        {
            foreach (Layerage child in children)
            {
                Transformer transformer = child.GetActualTransformer();
                drawingSession.DrawBound(transformer, matrix);
            }
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)=> null;
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Group");
        }

    }
}