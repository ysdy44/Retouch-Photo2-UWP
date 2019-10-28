using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using System.Xml.Linq;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s SaturationAdjustment.
    /// </summary>
    public class SaturationAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> Gets or sets the saturation intensity for effect. </summary>
        public float Saturation = 1.0f;


        //@Construct
        /// <summary>
        /// Construct a saturation-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public SaturationAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a saturation-adjustment.
        /// </summary>
        public SaturationAdjustment()
        {
        }


        public void Reset()
        {
            this.Saturation = 1.0f;
        }
        public IAdjustment Clone()
        {
            return new SaturationAdjustment
            {
                Saturation = this.Saturation,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "Saturation",
                new XAttribute("Saturation", this.Saturation)
            );
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