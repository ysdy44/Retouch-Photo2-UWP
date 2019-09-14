using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HighlightsAndShadowsAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class HighlightsAndShadowsAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.HighlightsAndShadows.ToString();
        public AdjustmentType Type => AdjustmentType.HighlightsAndShadows;
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        public Visibility PageVisibility => Visibility.Visible;


        [JsonProperty]
        public float Shadows;
        [JsonProperty]
        public float Highlights;
        [JsonProperty]
        public float Clarity;
        [JsonProperty]
        public float MaskBlurAmount;
        [JsonProperty]
        public bool SourceIsLinearGamma;


        public void Reset()
        {
            this.Shadows = 0.0f;
            this.Highlights = 0.0f;
            this.Clarity = 0.0f;
            this.MaskBlurAmount = 1.25f;
            this.SourceIsLinearGamma = false;
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
    }
}