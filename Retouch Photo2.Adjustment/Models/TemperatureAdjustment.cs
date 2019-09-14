using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Icons;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s TemperatureAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TemperatureAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Temperature.ToString();
        public AdjustmentType Type => AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public Visibility PageVisibility => Visibility.Visible;


        [JsonProperty]
        public float Temperature;
        [JsonProperty]
        public float Tint;


        public void Reset()
        {
            this.Temperature = 0.0f;
            this.Tint = 0.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new TemperatureAndTintEffect
            {
                Temperature = this.Temperature,
                Tint = this.Tint,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new TemperatureAdjustment
            {
                Temperature = this.Temperature,
                Tint = this.Tint,
            };
        }
    }
}