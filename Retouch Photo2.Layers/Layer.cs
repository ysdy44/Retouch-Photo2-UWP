using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Transformers;
using System.ComponentModel;
using System.Numerics;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Layer Classes.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {
        //@Abstract
        /// <summary>
        /// Gets a specific rended-layer..
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> image </returns>
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource previousImage, Matrix3x2 canvasToVirtualMatrix);


        //@Virtual
        /// <summary>
        /// Sets layer's fill-color.
        /// </summary>
        /// <param name="fillColor"> The destination fill-color. </param>
        public virtual void SetFillColor(Color fillColor) { }
        /// <summary>
        /// Gets layer's fill-color.
        /// </summary>
        /// <returns> Return **Null** if layer does not have fill-color. </returns>
        public virtual Color? GetFillColor() => null;


        /// <summary> <see cref = "Layer" />'s name. </summary>
        public string Name = "Layer";
        /// <summary> <see cref = "Layer" />'s icon. </summary>
        public UIElement Icon;
        /// <summary> <see cref = "Layer" />'s opacity. </summary>
        public float Opacity=1.0f;
        /// <summary> <see cref = "Layer" />'s blend type. </summary>
        public BlendType BlendType;

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

            if (currentLayer.BlendType != BlendType.Normal)
            {
                currentImage = Blend.Render
                (
                    currentImage,
                    previousImage,
                    currentLayer.BlendType
                );
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