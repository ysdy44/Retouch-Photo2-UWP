using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s GrayAdjustment.
    /// </summary>
    public class GrayAdjustment: Adjustment
    {
        public const string Name = "Gray";
        public GrayAdjustmentItem GrayAdjustmentItem = new GrayAdjustmentItem();

        //@Construct
        public GrayAdjustment()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new GrayControl();
            base.Item = this.GrayAdjustmentItem;
            base.Item.Reset();
            base.HasPage = false;
        }
    }
}