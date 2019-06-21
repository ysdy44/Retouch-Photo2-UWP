using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers.Controls;
using Retouch_Photo2.Transformers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GroupLayer .
    /// </summary>
    public class GroupLayer : Layer
    {
        //@Construct
        public GroupLayer()
        {
            base.Name = "Group";
        }

        //@Override
        public override UIElement GetIcon() => new GroupControl();
  
        public override Layer Clone(ICanvasResourceCreator resourceCreator)
        {       
            ObservableCollection<Layer> children  = new ObservableCollection<Layer>();
            foreach (Layer child in this.Children)
            {
                Layer cloneLayer = child.Clone(resourceCreator);//Clone
                children.Add(cloneLayer);//Add
            }

            return new GroupLayer
            {
                Name = base.Name,
                Opacity = base.Opacity,
                BlendType = base.BlendType,
                TransformerMatrix = base.TransformerMatrix,

                IsChecked = base.IsChecked,
                Visibility = base.Visibility,

                Children = children,
            };
        }

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        { 
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                foreach (Layer child in this.Children)
                {
                    if (child.Visibility == Visibility.Collapsed) continue;
                    if (child.Opacity ==0) continue;

                    //GetRender
                    ICanvasImage currentImage = child.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);
                    ds.DrawImage(currentImage);
                }
            }
            return command;
        }
    }
}