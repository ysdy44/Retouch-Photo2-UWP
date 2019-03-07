using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class GammaTransferAdjustmentItem : AdjustmentItem
    {
        public bool ClampOutput;

        public bool AlphaDisable;
        public float AlphaOffset;
        public float AlphaExponent;
        public float AlphaAmplitude;

        public bool RedDisable;
        public float RedOffset;
        public float RedExponent;
        public float RedAmplitude;

        public bool GreenDisable;
        public float GreenOffset;
        public float GreenExponent;
        public float GreenAmplitude;

        public bool BlueDisable;
        public float BlueOffset;
        public float BlueExponent;
        public float BlueAmplitude;

        public GammaTransferAdjustmentItem() => base.Type = AdjustmentType.GammaTransfer;

        public override Adjustment GetAdjustment() => new GammaTransferAdjustment()
        {
            GammaTransferAdjustmentItem = this
        };
    }
}
