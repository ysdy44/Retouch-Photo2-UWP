using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
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
        //@Generic
        public static IAdjustmentGenericPage<HighlightsAndShadowsAdjustment> GenericPage;// = new HighlightsAndShadowsPage();

        //@Content
        public AdjustmentType Type => AdjustmentType.HighlightsAndShadows;
        public Visibility PageVisibility => Visibility.Visible;
        public UIElement Page => HighlightsAndShadowsAdjustment.GenericPage.Self;
        public string Text => HighlightsAndShadowsAdjustment.GenericPage.Text;


        /// <summary> How much to increase or decrease the darker parts of the image.Default value 0, range -1 to 1. </summary>
        public float Shadows = 0.0f;
        public float StartingShadows { get; private set; }
        public void CacheShadows() => this.StartingShadows = this.Shadows;
        
        /// <summary> How much to increase or decrease the brighter parts of the image.Default value 0, range -1 to 1. </summary>
        public float Highlights = 0.0f;
        public float StartingHighlights { get; private set; }
        public void CacheHighlights() => this.StartingHighlights = this.Highlights;
        
        /// <summary> How much to increase or decrease the mid-tone contrast of the image.Default value 0, range -1 to 1. </summary>
        public float Clarity = 0.0f;
        public float StartingClarity { get; private set; }
        public void CacheClarity() => this.StartingClarity = this.Clarity;
        
        /// <summary> Controls the size of the region used around a pixel to classify it as highlight or shadow. Lower values result in more localized adjustments. Default value 1.25, range 0 to 10. </summary>
        public float MaskBlurAmount = 1.25f;
        public float StartingMaskBlurAmount { get; private set; }
        public void CacheMaskBlurAmount() => this.StartingMaskBlurAmount = this.MaskBlurAmount;
        
        /// <summary> Specifies whether the source image uses linear gamma as opposed to the default sRGB. </summary>
        public bool SourceIsLinearGamma = false;
        

        public void Reset()
        {
            this.Shadows = 0.0f;
            this.Highlights = 0.0f;
            this.Clarity = 0.0f;
            this.MaskBlurAmount = 1.25f;
            this.SourceIsLinearGamma = false;

            if (HighlightsAndShadowsAdjustment.GenericPage.Adjustment == this)
            {
                HighlightsAndShadowsAdjustment.GenericPage.Follow(this);
            }
        }
        public void Follow()
        {
            HighlightsAndShadowsAdjustment.GenericPage.Adjustment = this;
            HighlightsAndShadowsAdjustment.GenericPage.Follow(this);
        }
        public void Close()
        {
            HighlightsAndShadowsAdjustment.GenericPage.Adjustment = null;
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