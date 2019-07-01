using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s InvertAdjustment.
    /// </summary>
    public class InvertAdjustment : Adjustment
    {
        public const string Name = "Invert";
        public InvertAdjustmentItem InvertAdjustmentItem = new InvertAdjustmentItem();

        //@Construct
        public InvertAdjustment()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            base.Item = this.InvertAdjustmentItem;
            base.Item.Reset();
            base.HasPage = false;
        }
    }
}