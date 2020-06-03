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

        public bool IsRefactoringRender { get; set; } = true;

        public bool IsRefactoringIconRender { get; set; } = true;


        ICanvasImage Render2 = null;

        public ICanvasImage GetActualRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            if (this.Render2 == null || this.IsRefactoringRender)
            {
                this.IsRefactoringRender = false;

                {
                    //Layer
                    ICanvasImage currentImage = this.GetRender(resourceCreator, children);

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
                    this.Control.IconRender = this.Render2.GetHeightTransformEffect(resourceCreator, LayerageCollection.ControlsHeight);
                }

            }
            return this.Render2;
        }

        //@Abstract
        public virtual ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                if (this.Transform.IsCrop)
                {
                    CanvasGeometry geometryCrop = this.Transform.CropTransformer.ToRectangle(resourceCreator);
                    
                    using (drawingSession.CreateLayer(1, geometryCrop))
                    {
                        this._render(resourceCreator, drawingSession, children);
                    }
                }
                else
                {
                    this._render(resourceCreator, drawingSession, children);
                }
            }
            return command;
        }

        private void _render(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, IList<Layerage> children)
        {
            //Stroke
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            // Fill a geometry with style.
            if (this.Style.Fill.Type != BrushType.None)
            {
                ICanvasBrush canvasBrush = this.Style.Fill.GetICanvasBrush(resourceCreator);
                drawingSession.FillGeometry(geometry, canvasBrush);
            }

            //CanvasActiveLayer
            if (children.Count != 0)
            {
                using (drawingSession.CreateLayer(1, geometry))
                {
                    ICanvasImage childImage = LayerBase.Render(resourceCreator, children);
                    drawingSession.DrawImage(childImage);
                }
            }

            //Stroke
            // Draw a geometry with style.
            if (this.Style.Stroke.Type != BrushType.None)
            {
                if (this.Style.StrokeWidth != 0)
                {
                    ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
                    float strokeWidth = this.Style.StrokeWidth;
                    drawingSession.DrawGeometry(geometry, canvasBrush, strokeWidth, this.Style.StrokeStyle);
                }
            }
        }
        

        public virtual void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, IList<Layerage> children, Windows.UI.Color accentColor)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator, matrix);
            drawingSession.DrawGeometry(geometry, accentColor);
        }


        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator);
        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);


        public virtual bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;

            Transformer transformer = layerage.GetActualTransformer();

            return transformer.FillContainsPoint(point);
        }

                     
        //@Static
        /// <summary>
        /// Render layers.
        /// </summary>  
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="children"> The children layerage. </param>
        /// <returns> The render image. </returns>
        public static ICanvasImage Render(ICanvasResourceCreator resourceCreator, IList<Layerage> layerages)
        {
            ICanvasImage previousImage = null;

            for (int i = layerages.Count - 1; i >= 0; i--)
            {
                Layerage currentLayerage = layerages[i];
                ILayer currentLayer = currentLayerage.Self;

                if (currentLayer.Visibility == Visibility.Collapsed) continue;
                if (currentLayer.Opacity == 0) continue;


                //Layer
                ICanvasImage currentImage = currentLayer.GetActualRender(resourceCreator, currentLayerage.Children);
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