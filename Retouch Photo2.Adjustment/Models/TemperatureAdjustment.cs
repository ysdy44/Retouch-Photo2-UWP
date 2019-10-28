using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s TemperatureAdjustment.
    /// </summary>
    public class TemperatureAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> Specifies how much to increase or decrease the temperature of the image. Default value 0, range -1 to 1. </summary>
        public float Temperature = 0.0f;
        /// <summary> Specifies how much to increase or decrease the tint of the image. Default value 0, range -1 to 1. </summary>
        public float Tint = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a temperature-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public TemperatureAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a temperature-adjustment.
        /// </summary>
        public TemperatureAdjustment()
        {
        }

      
        public void Reset()
        {
            this.Temperature = 0.0f;
            this.Tint = 0.0f;
        }
        public IAdjustment Clone()
        {
            return new TemperatureAdjustment
            {
                Temperature = this.Temperature,
                Tint = this.Tint,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "Temperature",
                new XAttribute("Temperature", this.Temperature),
                new XAttribute("Tint", this.Tint)
            );
        }
        public void Load(XElement element)
        {
            this.Temperature = (float)element.Attribute("Temperature");
            this.Tint = (float)element.Attribute("Tint");
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new TemperatureAndTintEffect
            {
                Temperature = this.Temperature,
                Tint = this.Tint,
                Source = image
            };
        }

    }
}