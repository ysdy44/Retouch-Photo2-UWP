// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GeometryLayer.
    /// </summary>
    public abstract class GeometryLayer : LayerBase
    {

        /// <summary>
        /// Gets a specific rended-layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="layerage"> The layerage. </param>
        /// <returns> The rendered layer. </returns>
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, Layerage layerage)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                if (this.Transform.IsCrop==false)
                {

                    switch (base.Style.Transparency.Type)
                    {
                        case BrushType.LinearGradient:
                        case BrushType.RadialGradient:
                        case BrushType.EllipticalGradient:
                            {
                                Transformer transformer = base.Transform.Transformer;
                                CanvasGeometry geometryCrop = transformer.ToRectangle(resourceCreator);
                                ICanvasBrush canvasBrush = this.Style.Transparency.GetICanvasBrush(resourceCreator);

                                using (drawingSession.CreateLayer(canvasBrush, geometryCrop))
                                {
                                    this.GetGeometryRender(resourceCreator, drawingSession, layerage);
                                }
                            }
                            break;
                        default:
                            this.GetGeometryRender(resourceCreator, drawingSession, layerage);
                            break;
                    }

                }
                else
                {
                    
                    Transformer cropTransformer = base.Transform.CropTransformer;
                    CanvasGeometry geometryCrop = cropTransformer.ToRectangle(resourceCreator);

                    switch (base.Style.Transparency.Type)
                    {
                        case BrushType.LinearGradient:
                        case BrushType.RadialGradient:
                        case BrushType.EllipticalGradient:
                            {
                                ICanvasBrush canvasBrush = this.Style.Transparency.GetICanvasBrush(resourceCreator);

                                using (drawingSession.CreateLayer(canvasBrush, geometryCrop))
                                {
                                    this.GetGeometryRender(resourceCreator, drawingSession, layerage);
                                }
                            }
                            break;
                        default:
                            using (drawingSession.CreateLayer(1, geometryCrop))
                            {
                                this.GetGeometryRender(resourceCreator, drawingSession, layerage);
                            }
                            break;
                    }

                }
            }
            return command;
        }
        private void GetGeometryRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Layerage layerage)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);
            this.Geometry2 = geometry;

            if (this.Style.IsStrokeBehindFill == false)
            {


                // Fill
                // Fill a geometry with style.
                if (this.Style.Fill.Type != BrushType.None)
                {
                    ICanvasBrush canvasBrush = this.Style.Fill.GetICanvasBrush(resourceCreator);
                    drawingSession.FillGeometry(geometry, canvasBrush);
                }

                // CanvasActiveLayer
                if (layerage.Children.Count != 0)
                {
                    using (drawingSession.CreateLayer(1, geometry))
                    {
                        ICanvasImage childImage = LayerBase.Render(resourceCreator, layerage);

                        if ((childImage is null) == false)
                        {
                            drawingSession.DrawImage(childImage);
                        }
                    }
                }

                // Stroke
                // Draw a geometry with style.
                if (this.Style.Stroke.Type != BrushType.None)
                {
                    if (this.Style.StrokeWidth != 0)
                    {
                        ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
                        float strokeWidth = this.Style.StrokeWidth;
                        CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;
                        drawingSession.DrawGeometry(geometry, canvasBrush, strokeWidth, strokeStyle);
                    }
                }


            }
            else
            {


                // Stroke
                // Draw a geometry with style.
                if (this.Style.Stroke.Type != BrushType.None)
                {
                    if (this.Style.StrokeWidth != 0)
                    {
                        ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
                        float strokeWidth = this.Style.StrokeWidth;
                        CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;
                        drawingSession.DrawGeometry(geometry, canvasBrush, strokeWidth, strokeStyle);
                    }
                }

                // CanvasActiveLayer
                if (layerage.Children.Count != 0)
                {
                    using (drawingSession.CreateLayer(1, geometry))
                    {
                        ICanvasImage childImage = LayerBase.Render(resourceCreator, layerage);

                        if ((childImage is null) == false)
                        {
                            drawingSession.DrawImage(childImage);
                        }
                    }
                }

                // Fill
                // Fill a geometry with style.
                if (this.Style.Fill.Type != BrushType.None)
                {
                    ICanvasBrush canvasBrush = this.Style.Fill.GetICanvasBrush(resourceCreator);
                    drawingSession.FillGeometry(geometry, canvasBrush);
                }


            }
        }
        private CanvasGeometry Geometry2
        {
            get => this.geometry2;
            set => this.geometry2 = value;
        }
        private CanvasGeometry geometry2 = null;


        /// <summary>
        /// Draw wireframe.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public override void DrawWireframe(CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            if (this.Geometry2 is null) return;
            drawingSession.DrawGeometry(this.Geometry2.Transform(matrix), accentColor);
        }


        /// <summary>
        ///Returns whether the area filled by the layer contains the specified point.
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        /// <param name="point"> The point. </param>
        /// <returns> If the fill contains points, return **True**. </returns>
        public override bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;
            if (this.Geometry2 is null) return false;

            if (this.Style.Fill.Type != BrushType.None)
            {
                return this.Geometry2.FillContainsPoint(point);
            }

            if (this.Style.Stroke.Type != BrushType.None)
            {
                if (this.Style.StrokeWidth != 0)
                {
                    return this.Geometry2.StrokeContainsPoint(point, this.Style.StrokeWidth, this.Style.StrokeStyle);
                }
            }

            return false;
        }


        /// <summary>
        ///  Convert to curves.
        /// </summary>
        /// <returns> The product curves. </returns>
        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            return new NodeCollection(geometry);
        }

    }
}