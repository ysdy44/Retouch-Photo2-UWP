using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class Layer : ILayer, INotifyPropertyChanged
    {

        //@Abstract
        public abstract string Type { get; }
        public abstract UIElement Icon { get; }
        public virtual Color? FillColor { get; set; } = null;
        public virtual Color? StrokeColor { get; set; } = null;


        public string Name { get; set; } = string.Empty;
        public float Opacity { get; set; } = 1.0f;
        public BlendType BlendType { get; set; } = BlendType.Normal;


        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);
        public Transformer Source { get; set; }
        public Transformer Destination { get; set; }
        public Transformer OldDestination { get; set; }
        public bool DisabledRadian { get; set; } = false;

        public ObservableCollection<ILayer> Children { get; protected set; } = new ObservableCollection<ILayer>();
        public EffectManager EffectManager { get; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; } = new AdjustmentManager();



        //@Abstract
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix);
        public virtual void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.Destination.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.Destination.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.Destination.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.Destination.LeftBottom, matrix);

            drawingSession.DrawLine(leftTop, rightTop, accentColor);
            drawingSession.DrawLine(rightTop, rightBottom, accentColor);
            drawingSession.DrawLine(rightBottom, leftBottom, accentColor);
            drawingSession.DrawLine(leftBottom, leftTop, accentColor);
        }

        public abstract ILayer Clone(ICanvasResourceCreator resourceCreator);


        //@Abstract
        public virtual void CacheTransform() => this.OldDestination = this.Destination;
        public virtual void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.OldDestination * matrix;
        public virtual void TransformAdd(Vector2 vector) => this.Destination = this.OldDestination + vector;


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