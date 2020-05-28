using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ExposureAdjustment.
    /// </summary>
    public class ExposureAdjustment : IAdjustment
    {
        //@Static
        //@Generic
        public static IAdjustmentGenericPage<ExposureAdjustment> GenericPage;// = new ExposurePage();
        
        //@Content
        public AdjustmentType Type => AdjustmentType.Exposure;
        public Visibility PageVisibility => Visibility.Visible;
        public UIElement Page => ExposureAdjustment.GenericPage.Self;
        public string Text => ExposureAdjustment.GenericPage.Text;


        /// <summary> How much to increase or decrease the exposure of the image.Default value 0, range -2 -> 2. </summary>
        public float Exposure = 0.0f;
        public float StartingExposure { get; private set; }
        public void CacheExposure() => this.StartingExposure = this.Exposure;
        

        public void Reset()
        {
            this.Exposure = 0.0f;

            if (ExposureAdjustment.GenericPage.Adjustment == this)
            {
                ExposureAdjustment.GenericPage.Reset();
            }
        }
        public void Follow()
        {
            ExposureAdjustment.GenericPage.Adjustment = this;
            ExposureAdjustment.GenericPage.Follow(this);
        }
        public void Close()
        {
            ExposureAdjustment.GenericPage.Adjustment = null;
        }


        public IAdjustment Clone()
        {
            return new ExposureAdjustment
            {
                Exposure = this.Exposure,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Exposure", this.Exposure));
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