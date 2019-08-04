using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Controls;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s VignetteAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class VignetteAdjustment : IAdjustment
    {
        [JsonProperty] public string TypeName { get; } = AdjustmentType.Vignette.ToString();
        public AdjustmentType Type => AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteControl();
        public Visibility Visibility => Visibility.Visible;


        [JsonProperty]
        public float Amount;
        [JsonProperty]
        public float Curve;
        [JsonProperty]
        public Color Color;


        public void Reset()
        {
            this.Amount = 0.0f;
            this.Curve = 0.0f;
            this.Color = Colors.Black;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new VignetteEffect
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new VignetteAdjustment
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
            };
        }
    }
}