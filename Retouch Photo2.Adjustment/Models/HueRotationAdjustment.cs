using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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
        //@Generic
        public static IAdjustmentGenericPage<HueRotationAdjustment> GenericPage;// = new HueRotationPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.HueRotation;
        public Visibility PageVisibility => Visibility.Visible;
        public UIElement Page => HueRotationAdjustment.GenericPage.Self;
        public string Text => HueRotationAdjustment.GenericPage.Text;


        /// <summary> Angle to rotate the hue, in radians. Default value 0, range 0 to 2*pi. </summary>
        public float Angle = 0.0f;
        public float StartingAngle { get; private set; }
        public void CacheAngle() => this.StartingAngle = this.Angle;


        public void Reset()
        {
            this.Angle = 0.0f;

            if (HueRotationAdjustment.GenericPage.Adjustment == this)
            {
                HueRotationAdjustment.GenericPage.Reset();
            }
        }
        public void Follow()
        {
            HueRotationAdjustment.GenericPage.Adjustment = this;
            HueRotationAdjustment.GenericPage.Follow(this);
        }
        public void Close()
        {
            HueRotationAdjustment.GenericPage.Adjustment = null;
        }


        public IAdjustment Clone()
        {
            return new HueRotationAdjustment
            {
                Angle = this.Angle,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Angle", this.Angle));
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