using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : LayerBase
    {
        //@Construct
        public GroupLayer(LayerCollection layerCollection) : base(layerCollection)
        {
            base.Control.Icon = new GroupIcon();
        }

        //@Override
        public override string Type => "Group";
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        { 
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                foreach (ILayer child in this.Children)
                {
                    if (child.Visibility == Visibility.Collapsed) continue;
                    if (child.Opacity == 0) continue;

                    //GetRender
                    ICanvasImage currentImage = child.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);
                    drawingSession.DrawImage(currentImage);
                }
            }
            return command;
        }

        public override ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator)
        {
            GroupLayer groupLayer = new GroupLayer(layerCollection);

            LayerBase.CopyWith(layerCollection, resourceCreator, groupLayer, this);
            return groupLayer;
        }
    }
}