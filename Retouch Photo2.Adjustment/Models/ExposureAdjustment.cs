using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s ExposureAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ExposureAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Exposure.ToString();
        public AdjustmentType Type => AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon(); 
        public Visibility PageVisibility => Visibility.Visible;


        [JsonProperty]
        public float Exposure;


        public void Reset()
        {
            this.Exposure = 0.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure = this.Exposure,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new ExposureAdjustment
            {
                Exposure = this.Exposure,
            };
        }
    }
}