using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// Type of <see cref="PatternGridLayer"/>.
    /// </summary>
    public enum PatternGridType
    {
        /// <summary> Grid. </summary>
        Grid,

        /// <summary> Horizontal. </summary>
        Horizontal,

        /// <summary> Vertical. </summary>
        Vertical
    }

    /// <summary>
    /// <see cref="LayerBase"/>'s PatternGridLayer .
    /// </summary>
    public class PatternGridLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.PatternGrid;

        //@Content   
        public PatternGridType GridType = PatternGridType.Grid;

        public float HorizontalStep = 30.0f;
        public float StartingHorizontalStep { get; private set; }
        public void CacheHorizontalStep() => this.StartingHorizontalStep = this.HorizontalStep;

        public float VerticalStep = 30.0f;
        public float StartingVerticalStep { get; private set; }
        public void CacheVerticalStep() => this.StartingVerticalStep = this.VerticalStep;

        //@Construct
        /// <summary>
        /// Initializes a grid-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public PatternGridLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            PatternGridLayer gridLayer = new PatternGridLayer(customDevice)
            {
                GridType = this.GridType,
                HorizontalStep = this.HorizontalStep,
                VerticalStep = this.VerticalStep,
            };

            LayerBase.CopyWith(customDevice, gridLayer, this);
            return gridLayer;
        }


        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("GridType", this.GridType));
            element.Add(new XElement("HorizontalStep", this.HorizontalStep));
            element.Add(new XElement("VerticalStep", this.VerticalStep));
        }
        public override void Load(XElement element)
        {
            this.GridType = XML.CreatePatternGridMode(element.Element("GridType").Value);
            this.HorizontalStep = (float)element.Element("HorizontalStep");
            this.VerticalStep = (float)element.Element("VerticalStep");
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, IList<Layerage> children)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

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
                        ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
                        float strokeWidth = this.Style.StrokeWidth;
                        CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;

                        Transformer transformer = base.Transform.GetActualTransformer();
                        TransformerBorder border = new TransformerBorder(transformer);

                        if (this.GridType != PatternGridType.Vertical)
                        {
                            for (float i = border.Left; i < border.Right; i += this.HorizontalStep)
                            {
                                drawingSession.DrawLine(i, border.Top, i, border.Bottom, canvasBrush, strokeWidth, strokeStyle);
                            }
                        }

                        if (this.GridType != PatternGridType.Horizontal)
                        {
                            for (float i = border.Top; i < border.Bottom; i += this.VerticalStep)
                            {
                                drawingSession.DrawLine(border.Left, i, border.Right, i, canvasBrush, strokeWidth, strokeStyle);
                            }
                        }
                    }
                }
            }
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return transformer.ToRectangle(resourceCreator, matrix);
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator) => null;
        

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/PatternGrid");
        }

    }
}