using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s BrightnessAdjustment.
    /// </summary>
    public class BrightnessAdjustment : Adjustment
    {
        public const string Name = "Brightness";
        public BrightnessAdjustmentItem BrightnessAdjustmentItem = new BrightnessAdjustmentItem();

        //@Construct
        public BrightnessAdjustment()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            base.Item = this.BrightnessAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}