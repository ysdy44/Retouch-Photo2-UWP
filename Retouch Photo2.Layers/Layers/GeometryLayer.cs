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
        
        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                if (this.Transform.IsCrop)
                {
                    CanvasGeometry geometryCrop = this.Transform.CropTransformer.ToRectangle(resourceCreator);

                    using (drawingSession.CreateLayer(1, geometryCrop))
                    {
                        this.GetGeometryRender(resourceCreator, drawingSession, children);
                    }
                }
                else
                {
                    this.GetGeometryRender(resourceCreator, drawingSession, children);
                }
            }
            return command;
        }
        private void GetGeometryRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, IList<Layerage> children)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);
            this.Geometry2 = geometry;

            //Fill
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

                    if (childImage != null)
                    {
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
                    CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;
                    drawingSession.DrawGeometry(geometry, canvasBrush, strokeWidth, strokeStyle);
                }
            }
        }
        CanvasGeometry Geometry2 = null;


        public override void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, IList<Layerage> children, Windows.UI.Color accentColor)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator, matrix);
            drawingSession.DrawGeometry(geometry, accentColor);
        }
        

        public bool FillContainsPoint(Layerage layerage, Vector2 point)
        {
            if (this.Visibility == Visibility.Collapsed) return false;
            if (this.Geometry2 == null) return false;

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


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            return new NodeCollection(geometry);
        }

    }
}