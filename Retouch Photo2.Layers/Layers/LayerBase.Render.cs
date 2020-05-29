using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
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
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        Layerage child = children[i];
                        ILayer child2 = child.Self;

                        ICanvasImage childImage = child2.GetRender(resourceCreator, child.Children);
                        drawingSession.DrawImage(childImage);
                    }
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

        public abstract IEnumerable<IEnumerable<Node>> ConvertToCurves();


        public virtual bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;

            Transformer transformer = layerage.GetActualTransformer();

            return transformer.FillContainsPoint(point);
        }



        //@Static
        /// <summary>
        /// Render images and layers together.
        /// </summary>  
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="currentLayer"> The current layer. </param>
        /// <param name="previousImage"> Previous rendered images. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <param name="children"> The children layerage. </param>
        /// <returns> The rendered layer. </returns>
        public static ICanvasImage Render(ICanvasResourceCreator resourceCreator, ILayer currentLayer, ICanvasImage previousImage,  IList<Layerage> children)
        {
            if (currentLayer.Visibility == Visibility.Collapsed) return previousImage;
            if (currentLayer.Opacity == 0) return previousImage;

            //Layer
            ICanvasImage currentImage = currentLayer.GetRender(resourceCreator, children);

            //Effect
            currentImage = Effect.Render(currentLayer.Effect, currentImage);

            //Adjustment
            currentImage = Filter.Render(currentLayer.Filter, currentImage);

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
            if (currentLayer.BlendMode is BlendEffectMode blendMode)
            {
                currentImage = new BlendEffect
                {
                    Background = currentImage,
                    Foreground = previousImage,
                    Mode = blendMode
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