// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s TemperatureAdjustment.
    /// </summary>
    public class TemperatureAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static string GenericText = "Temperature";
        public static IAdjustmentPage GenericPage;// = new TemperaturePage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Temperature;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page { get; } = TemperatureAdjustment.GenericPage;
        public string Text => TemperatureAdjustment.GenericText;


        /// <summary> Specifies how much to increase or decrease the temperature of the image. Default value 0, range -1 to 1. </summary>
        public float Temperature = 0.0f;
        public float StartingTemperature { get; private set; }
        public void CacheTemperature() => this.StartingTemperature = this.Temperature;
        
        /// <summary> Specifies how much to increase or decrease the tint of the image. Default value 0, range -1 to 1. </summary>
        public float Tint = 0.0f;
        public float StartingTint { get; private set; }
        public void CacheTint() => this.StartingTint = this.Tint;
               
        
        public IAdjustment Clone()
        {
            return new TemperatureAdjustment
            {
                Temperature = this.Temperature,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Temperature", this.Temperature));
            element.Add(new XAttribute("Tint", this.Tint));
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