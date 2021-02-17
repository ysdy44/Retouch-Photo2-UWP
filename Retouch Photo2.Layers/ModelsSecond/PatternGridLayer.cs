// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
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
    public class PatternGridLayer : PatternLayer, ILayer
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

        
        public override ILayer Clone()
        {
            PatternGridLayer gridLayer = new PatternGridLayer
            {
                GridType = this.GridType,
                HorizontalStep = this.HorizontalStep,
                VerticalStep = this.VerticalStep,
            };

            LayerBase.CopyWith(gridLayer, this);
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
            if (element.Element("GridType") is XElement gridType) this.GridType = XML.CreatePatternGridMode(gridType.Value);
            if (element.Element("HorizontalStep") is XElement horizontalStep) this.HorizontalStep = (float)horizontalStep;
            if (element.Element("VerticalStep") is XElement verticalStep) this.VerticalStep = (float)verticalStep;
        }


        public override void GetPatternRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry)
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
        

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_PatternGrid");
        }

    }
}