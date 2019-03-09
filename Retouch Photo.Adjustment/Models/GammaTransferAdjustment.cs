using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class GammaTransferAdjustment : Adjustment
    {
        public static readonly string Name = "GammaTransfer";
        public GammaTransferAdjustmentItem GammaTransferAdjustmentItem = new GammaTransferAdjustmentItem();

        public GammaTransferAdjustment()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            base.Item = this.GammaTransferAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.GammaTransferAdjustmentItem.ClampOutput = false;

            this.GammaTransferAdjustmentItem.AlphaDisable = true;
            this.GammaTransferAdjustmentItem.AlphaOffset = 0;
            this.GammaTransferAdjustmentItem.AlphaExponent = 1;
            this.GammaTransferAdjustmentItem.AlphaAmplitude = 1;

            this.GammaTransferAdjustmentItem.RedDisable = true;
            this.GammaTransferAdjustmentItem.RedOffset = 0;
            this.GammaTransferAdjustmentItem.RedExponent = 1;
            this.GammaTransferAdjustmentItem.RedAmplitude = 1;

            this.GammaTransferAdjustmentItem.GreenDisable = true;
            this.GammaTransferAdjustmentItem.GreenOffset = 0;
            this.GammaTransferAdjustmentItem.GreenExponent = 1;
            this.GammaTransferAdjustmentItem.GreenAmplitude = 1;

            this.GammaTransferAdjustmentItem.BlueDisable = true;
            this.GammaTransferAdjustmentItem.BlueOffset = 0;
            this.GammaTransferAdjustmentItem.BlueExponent = 1;
            this.GammaTransferAdjustmentItem.BlueAmplitude = 1;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new GammaTransferEffect
            {
                ClampOutput = this.GammaTransferAdjustmentItem.ClampOutput,

                AlphaDisable = this.GammaTransferAdjustmentItem.AlphaDisable,
                AlphaOffset = this.GammaTransferAdjustmentItem.AlphaOffset,
                AlphaExponent = this.GammaTransferAdjustmentItem.AlphaExponent,
                AlphaAmplitude = this.GammaTransferAdjustmentItem.AlphaAmplitude,

                RedDisable = this.GammaTransferAdjustmentItem.RedDisable,
                RedOffset = this.GammaTransferAdjustmentItem.RedOffset,
                RedExponent = this.GammaTransferAdjustmentItem.RedExponent,
                RedAmplitude = this.GammaTransferAdjustmentItem.RedAmplitude,

                GreenDisable = this.GammaTransferAdjustmentItem.GreenDisable,
                GreenOffset = this.GammaTransferAdjustmentItem.GreenOffset,
                GreenExponent = this.GammaTransferAdjustmentItem.GreenExponent,
                GreenAmplitude = this.GammaTransferAdjustmentItem.GreenAmplitude,

                BlueDisable = this.GammaTransferAdjustmentItem.BlueDisable,
                BlueOffset = this.GammaTransferAdjustmentItem.BlueOffset,
                BlueExponent = this.GammaTransferAdjustmentItem.BlueExponent,
                BlueAmplitude = this.GammaTransferAdjustmentItem.BlueAmplitude,
                
                Source = image
            };
        }
    }
}