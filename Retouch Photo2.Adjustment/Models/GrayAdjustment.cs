using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="IAdjustment"/>'s GammaTransferAdjustment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GrayAdjustment : IAdjustment
    {
        [JsonProperty]
        public string TypeName { get; } = AdjustmentType.Gray.ToString();
         public AdjustmentType Type => AdjustmentType.Gray;
         public FrameworkElement Icon { get; } = new GrayControl();
        public Visibility Visibility => Visibility.Collapsed;


        public void Reset() { }
        public ICanvasImage GetRender(ICanvasImage image)
        {
            return new GrayscaleEffect
            {
                Source = image
            };
        }
        public IAdjustment Clone()
        {
            return new GrayAdjustment();
        }
    }
}