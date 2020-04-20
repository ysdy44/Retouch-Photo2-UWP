using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
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
        public static readonly ExposurePage ExposurePage = new ExposurePage();

        //@Content
        public AdjustmentType Type => AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => ExposureAdjustment.ExposurePage;
        public string Text { get; private set; }

        /// <summary> How much to increase or decrease the exposure of the image.Default value 0, range -2 -> 2. </summary>
        public float Exposure = 0.0f;


        //@Construct
        /// <summary>
        /// Construct a Exposure-adjustment.
        /// </summary>
        public ExposureAdjustment()
        {
            this.Text = ExposureAdjustment.ExposurePage.Text;
        }


        public void Reset()
        {
            this.Exposure = 0.0f;

            if (ExposureAdjustment.ExposurePage.Adjustment == this)
            {
                ExposureAdjustment.ExposurePage.Follow(this);
            }
        }
        public void Follow()
        {
            ExposureAdjustment.ExposurePage.Adjustment = this;
            ExposureAdjustment.ExposurePage.Follow(this);
        }
        public void Close()
        {
            ExposureAdjustment.ExposurePage.Adjustment = null;
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