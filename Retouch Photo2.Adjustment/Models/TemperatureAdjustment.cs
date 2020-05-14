using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        public static readonly TemperaturePage TemperaturePage = new TemperaturePage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => TemperatureAdjustment.TemperaturePage;
        public string Text { get; private set; }

        /// <summary> Specifies how much to increase or decrease the temperature of the image. Default value 0, range -1 to 1. </summary>
        public float Temperature = 0.0f;
        /// <summary> Specifies how much to increase or decrease the tint of the image. Default value 0, range -1 to 1. </summary>
        public float Tint = 0.0f;


        //@Construct
        /// <summary>
        /// Initializes a Temperature-adjustment.
        /// </summary>
        public TemperatureAdjustment()
        {
            this.Text = TemperatureAdjustment.TemperaturePage.Text;
        }


        public void Reset()
        {
            this.Temperature = 0.0f;
            this.Tint = 0.0f;

            if (TemperatureAdjustment.TemperaturePage.Adjustment == this)
            {
                TemperatureAdjustment.TemperaturePage.Follow(this);
            }
        }
        public void Follow()
        {
            TemperatureAdjustment.TemperaturePage.Adjustment = this;
            TemperatureAdjustment.TemperaturePage.Follow(this);
        }
        public void Close()
        {
            TemperatureAdjustment.TemperaturePage.Adjustment = null;
        }


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