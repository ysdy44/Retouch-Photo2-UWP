using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Effects;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// <see cref="LayersControl"/>items's Model Class.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {
        //@Abstract
        /// <summary>
        /// Get a specific rended-layer..
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> image </returns>
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource previousImage, Matrix3x2 canvasToVirtualMatrix);


        /// <summary> <see cref = "Layer" />'s name. </summary>
        public string Name = "Layer";
        /// <summary> <see cref = "Layer" />'s icon. </summary>
        public UIElement Icon;
        /// <summary> <see cref = "Layer" />'s opacity. </summary>
        public float Opacity=1.0f;


        /// <summary> <see cref = "Layer" />'s TransformerMatrix. </summary>
        public TransformerMatrix TransformerMatrix;

        //@Static
        /// <summary>
        /// Render images and layers together.
        /// </summary>  
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="currentLayer"> The current layer. </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> image </returns>
        public static ICanvasImage Render(ICanvasResourceCreator resourceCreator, Layer currentLayer, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            if (currentLayer.IsVisual == false || currentLayer.Opacity == 0) return previousImage;

            ICanvasImage currentImage = currentLayer.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);

            if (currentLayer.Opacity < 1.0)
            {
                currentImage= new OpacityEffect
                {
                    Opacity = currentLayer.Opacity,
                    Source = currentImage
                };
            } 

            return new CompositeEffect
            {
                Sources =
                {
                    previousImage,
                    currentImage,
                }
            };
        }
    }
}