using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Icons;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : Layer
    {
        //@Override
        public override string Type => "Group";
        public override UIElement Icon => new GroupIcon();
  
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        { 
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                foreach (ILayer child in this.Children)
                {
                    if (child.Visibility == Visibility.Collapsed) continue;
                    if (child.Opacity ==0) continue;

                    //GetRender
                    ICanvasImage currentImage = child.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);
                    drawingSession.DrawImage(currentImage);
                }
            }
            return command;
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {       
            ObservableCollection<ILayer> children  = new ObservableCollection<ILayer>();
            foreach (ILayer child in this.Children)
            {
                ILayer cloneLayer = child.Clone(resourceCreator);//Clone
                children.Add(cloneLayer);//Add
            }

            return new GroupLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,
                
                Source = base.Source,
                Destination = base.Destination,
                DisabledRadian = base.DisabledRadian,

                Children = children,
            };
        }
    }
}