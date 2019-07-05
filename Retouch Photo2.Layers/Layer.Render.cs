using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
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
        /// Draw lines on bound.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        public virtual void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession ds, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.Destination.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.Destination.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.Destination.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.Destination.LeftBottom, matrix);

            ds.DrawLine(leftTop, rightTop, accentColor);
            ds.DrawLine(rightTop, rightBottom, accentColor);
            ds.DrawLine(rightBottom, leftBottom, accentColor);
            ds.DrawLine(leftBottom, leftTop, accentColor);
        }




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
            if (currentLayer.Visibility == Visibility.Collapsed) return previousImage;
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
                currentImage = new OpacityEffect
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
