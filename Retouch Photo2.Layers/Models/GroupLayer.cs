using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;
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

        //@Construct
        /// <summary>
        /// Initializes a group-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GroupLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override Transformer GetActualTransformer(Layerage layerage)
        {
            //Refactoring
            if (this.IsRefactoringTransformer)
            {
                this.IsRefactoringTransformer = false;

                //TransformerBorder
                TransformerBorder border = new TransformerBorder(layerage.Children);
                Transformer transformer = border.ToTransformer();
                this.Transform.Transformer = transformer;
                return transformer;
            }

            return this.Transform.GetActualTransformer();
        }

        public override ILayer Clone(CanvasDevice customDevice)
        {
            GroupLayer groupLayer = new GroupLayer(customDevice);

            LayerBase.CopyWith(customDevice, groupLayer, this);
            return groupLayer;
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    Layerage child = children[i];
                    ILayer child2 = child.Self; 

                    if (child2.Visibility == Visibility.Collapsed) continue;
                    if (child2.Opacity == 0) continue;

                    //GetRender
                    ICanvasImage currentImage = child2.GetActualRender(resourceCreator, child.Children);
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

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator) => null;
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix) => null;
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/Group");
        }

    }
}