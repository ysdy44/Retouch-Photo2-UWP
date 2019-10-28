using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {
        
        //@Abstract
        public virtual void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            Transformer transformer = this.GetActualDestinationWithRefactoringTransformer;

            drawingSession.DrawBound(transformer, matrix, accentColor);
        }


        //@Static
        /// <summary>
        /// Render images and layers together.
        /// </summary>  
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="currentLayer"> The current layer. </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <returns> The rendered layer. </returns>
        public static ICanvasImage Render(ICanvasResourceCreator resourceCreator, ILayer currentLayer, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            if (currentLayer.Visibility == Visibility.Collapsed) return previousImage;
            if (currentLayer.Opacity == 0) return previousImage;

            //Layer
            ICanvasImage currentImage = currentLayer.GetRender(resourceCreator, previousImage, canvasToVirtualMatrix);

            //Transform
            currentImage = TransformManager.Render(currentLayer.TransformManager, resourceCreator, currentImage, canvasToVirtualMatrix);

            //Effect
            currentImage = EffectManager.Render(currentLayer.EffectManager, currentImage);

            //Adjustment
            currentImage = AdjustmentManager.GetRender(currentLayer.AdjustmentManager, currentImage);

            //Opacity
            if (currentLayer.Opacity < 1.0)
            {
                currentImage = new OpacityEffect
                {
                    Opacity = currentLayer.Opacity,
                    Source = currentImage
                };
            }

            //Blend
            if (currentLayer.BlendType != BlendType.None)
            {
                currentImage = BlendHelper.Render
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