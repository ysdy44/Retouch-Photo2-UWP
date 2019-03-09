using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class BrightnessAdjustmentItem : AdjustmentItem
    {       
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteLight;      
        /// <summary> Interval 1.0->0.5 . </summary>
        public float WhiteDark;

        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackLight;   
        /// <summary> Interval 0.0->0.5 . </summary>
        public float BlackDark;

        public BrightnessAdjustmentItem() => base.Name = BrightnessAdjustment.Name;

        public override Adjustment GetAdjustment() => new BrightnessAdjustment()
        {
            BrightnessAdjustmentItem = this
        };
    }
}
