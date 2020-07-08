using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s SaturationAdjustment.
    /// </summary>
    public class SaturationAdjustment : IAdjustment
    {
        //@Static      
        //@Generic
        public static string GenericText = "Saturation";
        public static IAdjustmentPage GenericPage;// = new SaturationPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Saturation;
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page { get; } = SaturationAdjustment.GenericPage;
        public string Text => SaturationAdjustment.GenericText;


        /// <summary> Gets or sets the saturation intensity for effect. </summary>
        public float Saturation = 1.0f;
        public float StartingSaturation { get; private set; }
        public void CacheSaturation() => this.StartingSaturation = this.Saturation;
        

        public IAdjustment Clone()
        {
            return new SaturationAdjustment
            {
                Saturation = this.Saturation,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Saturation", this.Saturation));
        }
        public void Load(XElement element)
        {
            this.Saturation = (float)element.Attribute("Saturation");
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }

    }
}