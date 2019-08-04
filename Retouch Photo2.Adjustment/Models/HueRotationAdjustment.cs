using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s HueRotationAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class HueRotationAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.HueRotation.ToString();
         public AdjustmentType Type => AdjustmentType.HueRotation;
         public FrameworkElement Icon { get; } = new HueRotationControl();
         public Visibility Visibility => Visibility.Visible;


        [JsonProperty]
        public float Angle;


        public void Reset()
        {
            this.Angle = 0.0f;
        }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new HueRotationEffect
            {
                Angle = this.Angle,
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new HueRotationAdjustment
            {
                Angle = this.Angle,
            };
        }
    }
}