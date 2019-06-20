using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Transformers;
using System.Collections.ObjectModel;
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
        /// Gets a specific rended-layer.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> image </returns>
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix);
        /// <summary>
        /// Gets layer's icon.
        /// </summary>
        /// <returns> icon </returns>
        public abstract UIElement GetIcon();

        /// <summary>
        /// Get layer own copy.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <returns></returns>
        public abstract Layer Clone(ICanvasResourceCreator resourceCreator);

        
        /// <summary> <see cref = "Layer" />'s name. </summary>
        public string Name = "Layer";
        /// <summary> <see cref = "Layer" />'s icon. </summary>
        public UIElement Icon=>this.GetIcon();
        /// <summary> <see cref = "Layer" />'s opacity. </summary>
        public float Opacity=1.0f;
        /// <summary> <see cref = "Layer" />'s blend type. </summary>
        public BlendType BlendType;

        /// <summary> <see cref = "Layer" />'s TransformerMatrix. </summary>
        public TransformerMatrix TransformerMatrix;
        /// <summary> <see cref = "Layer" />'s children layers. </summary>
        public ObservableCollection<Layer> Children = new ObservableCollection<Layer>();


        /// <summary> <see cref = "Layer" />'s EffectManager. </summary>
        public EffectManager EffectManager = new EffectManager();
        /// <summary> <see cref = "Layer" />'s AdjustmentManager. </summary>
        public AdjustmentManager AdjustmentManager = new AdjustmentManager();


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
            if (currentLayer.Visibility == Visibility.Collapsed ) return previousImage;
            if (currentLayer.Opacity == 0) return previousImage;

            //GetRender
            ICanvasImage currentImage = currentLayer.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);

            //Effect
            currentImage = EffectManager.Render(currentLayer.EffectManager, currentImage);

            //Adjustment
            currentImage = AdjustmentManager.Render(currentLayer.AdjustmentManager, currentImage);

            //Opacity
            if (currentLayer.Opacity < 1.0)
            {
                currentImage= new OpacityEffect
                {
                    Opacity = currentLayer.Opacity,
                    Source = currentImage
                };
            }

            //Blend
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