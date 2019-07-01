using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s SaturationAdjustment.
    /// </summary>
    public class SaturationAdjustment : Adjustment
    {
        public const string Name = "Saturation";
        public SaturationAdjustmentItem SaturationAdjustmentItem=new SaturationAdjustmentItem();

        //@Construct
        public SaturationAdjustment()
        {
            base.Type = AdjustmentType.Saturation;
            base.Icon = new SaturationControl();
            base.Item = this.SaturationAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}