using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using System.Numerics;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s BrightnessAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class BrightnessAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Brightness.ToString();
        public AdjustmentType Type => AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessControl();
        public Visibility Visibility => Visibility.Visible;

        /// <summary> Interval 1.0->0.5 . </summary>
        [JsonProperty]
        public float WhiteLight;
        /// <summary> Interval 1.0->0.5 . </summary>
        [JsonProperty]
        public float WhiteDark;

        /// <summary> Interval 0.0->0.5 . </summary>
        [JsonProperty]
        public float BlackLight;
        /// <summary> Interval 0.0->0.5 . </summary>
        [JsonProperty]
        public float BlackDark;


        public void Reset()
        {
            this.WhiteLight = 1.0f;
            this.WhiteDark = 1.0f;
            this.BlackLight = 1.0f;
            this.BlackDark = 1.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new BrightnessEffect
            {
                WhitePoint = new Vector2
                (
                    x: this.WhiteLight,
                    y: this.WhiteDark
                ),
                BlackPoint = new Vector2
                (
                    x: this.BlackDark,
                    y: this.BlackLight
                ),
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new BrightnessAdjustment
            {
                WhiteLight = this.WhiteLight,
                WhiteDark = this.WhiteDark,
                BlackLight = this.BlackLight,
                BlackDark = this.BlackDark,
            };
        }
    }
}