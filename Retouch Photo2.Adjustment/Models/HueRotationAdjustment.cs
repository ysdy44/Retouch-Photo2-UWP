using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HueRotationAdjustment.
    /// </summary>
    public class HueRotationAdjustment : IAdjustment
    {

        public AdjustmentType Type => AdjustmentType.HueRotation;
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        public Visibility PageVisibility => Visibility.Visible;

        /// <summary> Angle to rotate the hue, in radians. Default value 0, range 0 to 2*pi. </summary>
        public float Angle = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a hueRotation-adjustment.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public HueRotationAdjustment(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a hueRotation-adjustment.
        /// </summary>
        public HueRotationAdjustment()
        {            
        }

    
        public void Reset()
        {
            this.Angle = 0.0f;
        }
        public IAdjustment Clone()
        {
            return new HueRotationAdjustment
            {
                Angle = this.Angle,
            };
        }

        public XElement Save()
        {
            return new XElement
            (
                "HueRotation",
                new XAttribute("Angle", this.Angle)
            );
        }
        public void Load(XElement element)
        {
            this.Angle = (float)element.Attribute("Angle");
        }

        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new HueRotationEffect
            {
                Angle = this.Angle,
                Source = image
            };
        }

    }
}