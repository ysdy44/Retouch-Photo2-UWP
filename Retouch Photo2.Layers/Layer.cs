using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using Windows.Foundation;
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
        public BlendType BlendType { get; set; } = BlendType.None;


        public TransformManager TransformManager { get; set; } = new TransformManager();
        public EffectManager EffectManager { get; set; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; set; } = new AdjustmentManager();
        public ObservableCollection<ILayer> Children { get; set; } = new ObservableCollection<ILayer>();


        //@Abstract
        public virtual void CacheTransform() => this.TransformManager.CacheTransform();
        public virtual void TransformMultiplies(Matrix3x2 matrix) => this.TransformManager.TransformMultiplies(matrix);
        public virtual void TransformAdd(Vector2 vector) => this.TransformManager.TransformAdd(vector);


        //@Abstract
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix);
        public virtual void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            Transformer transformer = this.TransformManager.Destination;
            drawingSession.DrawBound(transformer, matrix, accentColor);
        }


        public abstract ILayer Clone(ICanvasResourceCreator resourceCreator);
        public void CopyWith(ICanvasResourceCreator resourceCreator, ILayer currentLayer)
        {
            currentLayer.Name = this.Name;
            currentLayer.Opacity = this.Opacity;
            currentLayer.BlendType = this.BlendType;

            currentLayer.IsChecked = this.IsChecked;
            currentLayer.Visibility = this.Visibility;

            currentLayer.TransformManager = this.TransformManager.Clone();
            currentLayer.EffectManager = this.EffectManager.Clone();
            
            foreach (IAdjustment adjustment in this.AdjustmentManager.Adjustments)
            {
                IAdjustment clone = adjustment.Clone(); 
                currentLayer.AdjustmentManager.Adjustments.Add(clone);
            }
            foreach (ILayer layer in this.Children)
            {
                ILayer clone = layer.Clone(resourceCreator);
                currentLayer.Children.Add(clone);
            }            
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
            currentImage = TransformManager.Render(currentLayer.TransformManager, resourceCreator,currentImage, canvasToVirtualMatrix);
            
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