using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s PatternLayer.
    /// </summary>
    public abstract class PatternLayer : LayerBase
    {

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            Transformer transformer = base.Transform.Transformer;

            CanvasGeometry geometry = transformer.ToRectangle(resourceCreator);


            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                if (this.Transform.IsCrop)
                {
                    CanvasGeometry geometryCrop = this.Transform.CropTransformer.ToRectangle(resourceCreator);

                    CanvasGeometryRelation relation = geometry.CompareWith(geometryCrop);
                    switch (relation)
                    {
                        case CanvasGeometryRelation.Disjoint:
                            return null;
                        case CanvasGeometryRelation.Contained:
                            this._render(resourceCreator, drawingSession, geometry);
                            break;
                        case CanvasGeometryRelation.Contains:
                            this._render(resourceCreator, drawingSession, geometryCrop);
                            break;
                        case CanvasGeometryRelation.Overlap:
                            Matrix3x2 zero = Matrix3x2.CreateTranslation(0.0f, 0.0f);
                            CanvasGeometry combine = geometry.CombineWith(geometryCrop, zero, CanvasGeometryCombine.Intersect);
                            this._render(resourceCreator, drawingSession, combine);
                            break;
                        default:
                            return null;
                    }
                }
                else
                {
                    this._render(resourceCreator, drawingSession, geometry);
                }
            }
            return command;
        }
        private void _render(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry)
        {

            //Fill
            // Fill a geometry with style.
            if (this.Style.Fill.Type != BrushType.None)
            {
                ICanvasBrush canvasBrush = this.Style.Fill.GetICanvasBrush(resourceCreator);
                drawingSession.FillGeometry(geometry, canvasBrush);
            }

            //CanvasActiveLayer
            using (drawingSession.CreateLayer(1, geometry))
            {

                //Stroke
                // Draw a geometry with style
                if (this.Style.Stroke.Type != BrushType.None)
                {
                    if (this.Style.StrokeWidth != 0)
                    {
                        this.GetPatternRender(resourceCreator, drawingSession, geometry);
                    }
                }
            }
        }
        public abstract void GetPatternRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry);


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;
        
    }
}