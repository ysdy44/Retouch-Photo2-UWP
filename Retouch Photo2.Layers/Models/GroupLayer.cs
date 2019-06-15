using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers.Controls;
using Retouch_Photo2.Transformers;
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
        /// <summary> <see cref = "GroupLayer" />'s children layers. </summary>
        public ObservableCollection<Layer> Children { get; private set; } = new ObservableCollection<Layer>();

        //@Construct
        public GroupLayer()
        {
            base.Name = "Group";
        }

        //@Override
        public override UIElement GetIcon() => new GroupControl();
        /*
         
      public override void CacheTransformerMatrix()
      {
            foreach (Layer child in this.Children)
            {
                child.CacheTransformerMatrix();
            }
        }
        public override void MultipliesTransformerMatrix(Matrix3x2 matrix)
        {
            foreach (Layer child in this.Children)
            {
                child.MultipliesTransformerMatrix(matrix);
            }
        }
             */

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
                Name = this.Name,
                Opacity = this.Opacity,
                BlendType = this.BlendType,
                TransformerMatrix = this.TransformerMatrix,

                IsChecked=this.IsChecked,
                Visibility=this.Visibility,

                Children = children
            };
        }

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            foreach (Layer child in this.Children)
            {
                previousImage = Layer.Render(resourceCreator, child, previousImage, canvasToVirtualMatrix);
            }
            return previousImage;
        }
    }
}