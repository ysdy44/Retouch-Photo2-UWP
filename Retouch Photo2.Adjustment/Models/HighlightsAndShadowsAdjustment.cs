using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Pages;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HighlightsAndShadowsAdjustment.
    /// </summary>
    public class HighlightsAndShadowsAdjustment : IAdjustment
    {
        //@Static
        public static readonly HighlightsAndShadowsPage HighlightsAndShadowsPage = new HighlightsAndShadowsPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.HighlightsAndShadows;
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        public Visibility PageVisibility => Visibility.Visible;
        public IAdjustmentPage Page => HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage;
        public string Text { get; private set; }

        /// <summary> How much to increase or decrease the darker parts of the image.Default value 0, range -1 to 1. </summary>
        public float Shadows = 0.0f;
        /// <summary> How much to increase or decrease the brighter parts of the image.Default value 0, range -1 to 1. </summary>
        public float Highlights = 0.0f;
        /// <summary> How much to increase or decrease the mid-tone contrast of the image.Default value 0, range -1 to 1. </summary>
        public float Clarity = 0.0f;
        /// <summary> Controls the size of the region used around a pixel to classify it as highlight or shadow. Lower values result in more localized adjustments. Default value 1.25, range 0 to 10. </summary>
        public float MaskBlurAmount = 1.25f;
        /// <summary> Specifies whether the source image uses linear gamma as opposed to the default sRGB. </summary>
        public bool SourceIsLinearGamma = false;


        //@Construct
        /// <summary>
        /// Initializes a HighlightsAndShadows-adjustment.
        /// </summary>
        public HighlightsAndShadowsAdjustment()
        {
            this.Text = HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Text;
        }


        public void Reset()
        {
            this.Shadows = 0.0f;
            this.Highlights = 0.0f;
            this.Clarity = 0.0f;
            this.MaskBlurAmount = 1.25f;
            this.SourceIsLinearGamma = false;

            if (HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Adjustment == this)
            {
                HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Follow(this);
            }
        }
        public void Follow()
        {
            HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Adjustment = this;
            HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Follow(this);
        }
        public void Close()
        {
            HighlightsAndShadowsAdjustment.HighlightsAndShadowsPage.Adjustment = null;
        }


        public IAdjustment Clone()
        {
            return new HighlightsAndShadowsAdjustment
            {
                Shadows = this.Shadows,
                Highlights = this.Highlights,
                Clarity = this.Clarity,
                MaskBlurAmount = this.MaskBlurAmount,
                SourceIsLinearGamma = this.SourceIsLinearGamma,
            };
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XAttribute("Shadows", this.Shadows));
            element.Add(new XAttribute("Highlights", this.Highlights));
            element.Add(new XAttribute("Clarity", this.Clarity));
            element.Add(new XAttribute("MaskBlurAmount", this.MaskBlurAmount));
            element.Add(new XAttribute("SourceIsLinearGamma", this.SourceIsLinearGamma));
        }
        public void Load(XElement element)
        {
            this.Shadows = (float)element.Attribute("Shadows");
            this.Highlights = (float)element.Attribute("Highlights");
            this.Clarity = (float)element.Attribute("Clarity");
            this.MaskBlurAmount = (float)element.Attribute("MaskBlurAmount");
            this.SourceIsLinearGamma = (bool)element.Attribute("SourceIsLinearGamma");
        }


        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new HighlightsAndShadowsEffect
            {
                Shadows = this.Shadows,
                Highlights = this.Highlights,
                Clarity = this.Clarity,
                MaskBlurAmount = this.MaskBlurAmount,
                SourceIsLinearGamma = this.SourceIsLinearGamma,
                Source = image
            };
        }

    }
}