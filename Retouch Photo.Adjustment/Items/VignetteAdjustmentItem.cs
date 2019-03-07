using Retouch_Photo.Adjustments.Models;
using Windows.UI;

namespace Retouch_Photo.Adjustments.Items
{
    public class VignetteAdjustmentItem : AdjustmentItem
    {
        public float Amount;
        public float Curve;
        public Color Color;

        public VignetteAdjustmentItem() => base.Type = AdjustmentType.Vignette;

        public override Adjustment GetAdjustment() => new VignetteAdjustment()
        {
            VignetteAdjustmentItem = this
        };
    }
}
