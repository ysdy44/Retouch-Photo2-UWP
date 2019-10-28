using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ExposureAdjustment.
    /// </summary>
    public class ExposureAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon(); 
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> How much to increase or decrease the exposure of the image.Default value 0, range -2 -> 2. </summary>
        public float Exposure;
   
        
        //@Construct
        /// <summary>
        /// Construct a exposure-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public ExposureAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a exposure-adjustment.
        /// </summary>
        public ExposureAdjustment()
        {
        }


        public void Reset()
        {
            this.Exposure = 0.0f;
        }
        public IAdjustment Clone()
        {
            return new ExposureAdjustment
            {
                Exposure = this.Exposure,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "Exposure",
                new XAttribute("Exposure", this.Exposure)
            );
        }
        public void Load(XElement element)
        {
            this.Exposure = (float)element.Attribute("Exposure");
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure = this.Exposure,
                Source = image
            };
        }

    }
}