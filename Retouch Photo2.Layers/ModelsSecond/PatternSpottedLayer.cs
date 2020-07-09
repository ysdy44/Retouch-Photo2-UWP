using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s PatternSpottedLayer .
    /// </summary>
    public class PatternSpottedLayer : PatternLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.PatternSpotted;

        //@Content   
        public float Radius = 8.0f;
        public float StartingRadius { get; private set; }
        public void CacheRadius() => this.StartingRadius = this.Radius;

        public float Step = 30.0f;
        public float StartingStep { get; private set; }
        public void CacheStep() => this.StartingStep = this.Step;

        //@Construct
        /// <summary>
        /// Initializes a spotted-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public PatternSpottedLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            PatternSpottedLayer spottedLayer = new PatternSpottedLayer(customDevice)
            {
                Radius = this.Radius,
            };

            LayerBase.CopyWith(customDevice, spottedLayer, this);
            return spottedLayer;
        }


        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Radius", this.Radius));
            element.Add(new XElement("Step", this.Step));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Radius") is XElement radius) this.Radius = (float)radius;
            if (element.Element("Step") is XElement step) this.Step = (float)step;
        }


        public override void GetPatternRender(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry)
        {
            ICanvasBrush canvasBrush = this.Style.Stroke.GetICanvasBrush(resourceCreator);
            float strokeWidth = this.Style.StrokeWidth;
            CanvasStrokeStyle strokeStyle = this.Style.StrokeStyle;

            Transformer transformer = base.Transform.GetActualTransformer();
            TransformerBorder border = new TransformerBorder(transformer);

            for (float y = border.Top; y < border.Bottom; y += this.Step)
            {
                for (float x = border.Left; x < border.Right; x += this.Step)
                {
                    drawingSession.FillCircle(x, y, this.Radius, canvasBrush);
                }
            }
        }
        

        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/PatternSpotted");
        }

    }
}