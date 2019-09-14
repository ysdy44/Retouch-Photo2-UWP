using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s SaturationAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SaturationAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Saturation.ToString();
        public AdjustmentType Type => AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public Visibility PageVisibility => Visibility.Visible;


        [JsonProperty]
        public float Saturation;


        public void Reset()
        {
            this.Saturation = 1.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new SaturationAdjustment
            {
                Saturation = this.Saturation,
            };
        }
    }
}