using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ContrastAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ContrastAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Contrast.ToString();
        public AdjustmentType Type => AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastIcon();
        public Visibility PageVisibility => Visibility.Visible;


        [JsonProperty]
        public float Contrast;


        public void Reset()
        {
            this.Contrast = 0.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ContrastEffect
            {
                Contrast = this.Contrast,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new ContrastAdjustment
            {
                Contrast = this.Contrast,
            };
        }
    }
}