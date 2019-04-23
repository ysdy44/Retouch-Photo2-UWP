using Retouch_Photo2.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
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

        public GammaTransferAdjustmentItem() => base.Name = GammaTransferAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new GammaTransferAdjustment()
        {
            GammaTransferAdjustmentItem = this
        };
        public override void Reset()
        {
            this.ClampOutput = false;

            this.AlphaDisable = true;
            this.AlphaOffset = 0;
            this.AlphaExponent = 1;
            this.AlphaAmplitude = 1;

            this.RedDisable = true;
            this.RedOffset = 0;
            this.RedExponent = 1;
            this.RedAmplitude = 1;

            this.GreenDisable = true;
            this.GreenOffset = 0;
            this.GreenExponent = 1;
            this.GreenAmplitude = 1;

            this.BlueDisable = true;
            this.BlueOffset = 0;
            this.BlueExponent = 1;
            this.BlueAmplitude = 1;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new GammaTransferEffect
            {
                ClampOutput = this.ClampOutput,

                AlphaDisable = this.AlphaDisable,
                AlphaOffset = this.AlphaOffset,
                AlphaExponent = this.AlphaExponent,
                AlphaAmplitude = this.AlphaAmplitude,

                RedDisable = this.RedDisable,
                RedOffset = this.RedOffset,
                RedExponent = this.RedExponent,
                RedAmplitude = this.RedAmplitude,

                GreenDisable = this.GreenDisable,
                GreenOffset = this.GreenOffset,
                GreenExponent = this.GreenExponent,
                GreenAmplitude = this.GreenAmplitude,

                BlueDisable = this.BlueDisable,
                BlueOffset = this.BlueOffset,
                BlueExponent = this.BlueExponent,
                BlueAmplitude = this.BlueAmplitude,

                Source = image
            };
        }
    }
}
