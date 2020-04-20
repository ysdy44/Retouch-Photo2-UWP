using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HueRotationAdjustment.
    /// </summary>
    public class HueRotationAdjustment : IAdjustment
    {
        //@Static
        public static readonly HueRotationPage HueRotationPage = new HueRotationPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.HueRotation;
        public FrameworkElement Icon { get; } = new HueRotationIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => HueRotationAdjustment.HueRotationPage;
        public string Text { get; private set; }

        /// <summary> Angle to rotate the hue, in radians. Default value 0, range 0 to 2*pi. </summary>
        public float Angle = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a HueRotation-adjustment.
        /// </summary>
        public HueRotationAdjustment()
        {
            this.Text = HueRotationAdjustment.HueRotationPage.Text;
        }


        public void Reset()
        {
            this.Angle = 0.0f;

            if (HueRotationAdjustment.HueRotationPage.Adjustment == this)
            {
                HueRotationAdjustment.HueRotationPage.Follow(this);
            }
        }
        public void Follow()
        {
            HueRotationAdjustment.HueRotationPage.Adjustment = this;
            HueRotationAdjustment.HueRotationPage.Follow(this);
        }
        public void Close()
        {
            HueRotationAdjustment.HueRotationPage.Adjustment = null;
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