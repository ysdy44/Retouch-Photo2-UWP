using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s InvertAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InvertAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Invert.ToString();
         public AdjustmentType Type => AdjustmentType.Invert;
         public FrameworkElement Icon { get; } = new InvertControl();
         public Visibility Visibility => Visibility.Visible;


        public void Reset() { }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect
            {
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new InvertAdjustment();
        }
    }
}