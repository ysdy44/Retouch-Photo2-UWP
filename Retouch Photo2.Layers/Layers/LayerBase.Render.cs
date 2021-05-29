using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Filters;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        /// <summary> Gets or sets ILayer is need to refactoring render. </summary>
        public bool IsRefactoringRender { get; set; } = true;

        /// <summary> Gets or sets ILayer is need to refactoring icon render. </summary>
        public bool IsRefactoringIconRender { get; set; } = true;


        /// <summary>
        /// Gets a specific actual rended-layer (with icon render).
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The rendered layer. </returns>
        public ICanvasImage GetActualRender(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            if (this.Render2 == null || this.IsRefactoringRender)
            {
                this.IsRefactoringRender = false;

                {
                    //Layer
                    ICanvasImage currentImage = this.GetRender(resourceCreator, layerage);

                    //Effect
                    currentImage = Effect.Render(this.Effect, currentImage);

                    //Adjustment
                    currentImage = Filter.Render(this.Filter, currentImage);

                    //Opacity
                    if (this.Opacity < 1.0)
                    {
                        currentImage = new OpacityEffect
                        {
                            Opacity = this.Opacity,
                            Source = currentImage
                        };
                    }

                    this.Render2 = currentImage;
                }


                if (this.IsRefactoringIconRender)
                {
                    this.IsRefactoringIconRender = false;

                    this.Control.IconRender = this.Render2?.ToIconRenderImage(resourceCreator, LayerManager.ControlsHeight);
                }

            }
            return this.Render2;
        }
        ICanvasImage Render2 = null;



        //@Abstract
        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The rendered layer. </returns>
        public abstract ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, Layerage layerage);


        /// <summary>
        /// Draw wireframe.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public virtual void DrawWireframe(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            Transformer transformer = this.Transform.GetActualTransformer();
            drawingSession.DrawBound(transformer, matrix);
        }


        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>  
        public virtual CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator) => null;
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>   
        public virtual CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => null;



        /// <summary>
        /// Returns whether the area filled by the layer contains the specified point.
        /// </summary>
        public virtual bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;

            Transformer transformer = layerage.GetActualTransformer();

            return transformer.FillContainsPoint(point);
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        public NodeCollection Nodes { get; protected set; } = null;
        /// <summary>
        /// Convert to curves layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product nodes. </returns>
        public virtual NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;



        //@Static
        /// <summary>
        /// Render layers.
        /// </summary>  
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The render image. </returns>
        public static ICanvasImage Render(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            ICanvasImage previousImage = null;

            for (int i = layerage.Children.Count - 1; i >= 0; i--)
            {
                Layerage currentLayerage = layerage.Children[i];
                ILayer currentLayer = currentLayerage.Self;

                if (currentLayer.Visibility == Visibility.Collapsed) continue;
                if (currentLayer.Opacity == 0) continue;


                //Layer
                ICanvasImage currentImage = currentLayer.GetActualRender(resourceCreator, currentLayerage);
                if (currentImage == null) continue;
                if (previousImage == null)
                {
                    previousImage = currentImage;
                    continue;
                }


                //Blend
                if (currentLayer.BlendMode is BlendEffectMode blendMode)
                {
                    previousImage = new BlendEffect
                    {
                        Background = currentImage,
                        Foreground = previousImage,
                        Mode = blendMode
                    };
                    continue;
                }

                //Composite
                previousImage = new CompositeEffect
                {
                    Sources =
                    {
                        previousImage,
                        currentImage,
                    }
                };
                continue;
            }

            return previousImage;
        }

    }
}