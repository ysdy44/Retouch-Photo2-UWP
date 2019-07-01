using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s HueRotationAdjustment.
    /// </summary>
    public class HueRotationAdjustment : Adjustment
    {
        public const string Name = "HueRotation";
        public HueRotationAdjustmentItem HueRotationAdjustmentitem=new HueRotationAdjustmentItem();

        //@Construct
        public HueRotationAdjustment()
        {
            base.Type = AdjustmentType.HueRotation;
            base.Icon = new HueRotationControl();
            base.Item = this.HueRotationAdjustmentitem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}