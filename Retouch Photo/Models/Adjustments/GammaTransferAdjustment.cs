using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentsControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class GammaTransferAdjustment : Adjustment
    {
        /// <summary>
        ///   If set, the effect clamps color values to between 0 and 1 before passing them on to the next effect in the graph. If false, the effect will not clamp values,
        ///   although subsequent effects or the output surface may later clamp if they are not of high enough precision.Default value false.
        /// </summary>
        public bool ClampOutput;

        public bool AlphaDisable=true;
        public float AlphaOffset;
        public float AlphaExponent = 1;
        public float AlphaAmplitude = 1;

        public bool RedDisable = true;
        public float RedOffset;
        public float RedExponent = 1;
        public float RedAmplitude = 1;

        public bool GreenDisable = true;
        public float GreenOffset;
        public float GreenExponent = 1;
        public float GreenAmplitude = 1;

        public bool BlueDisable = true;
        public float BlueOffset;
        public float BlueExponent = 1;
        public float BlueAmplitude = 1;

        public GammaTransferAdjustment()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            base.HasPage = true;
            this.Reset();
        }

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


    public class GammaTransferAdjustmentCandidate : AdjustmentCandidate
    {
        public GammaTransferPage page = new GammaTransferPage();

        public GammaTransferAdjustmentCandidate()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new GammaTransferAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is GammaTransferAdjustment GammaTransferAdjustment)
            {
                this.page.GammaTransferAdjustment = null;
                this.page.GammaTransferAdjustment = GammaTransferAdjustment;
            }
        }
    }
}