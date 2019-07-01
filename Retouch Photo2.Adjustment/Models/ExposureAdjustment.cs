using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s ExposureAdjustment.
    /// </summary>
    public class ExposureAdjustment : Adjustment
    {
        public const string Name = "Exposure";
        public ExposureAdjustmentItem ExposureAdjustmentItem=new ExposureAdjustmentItem();

        //@Construct
        public ExposureAdjustment()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            base.Item = this.ExposureAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}