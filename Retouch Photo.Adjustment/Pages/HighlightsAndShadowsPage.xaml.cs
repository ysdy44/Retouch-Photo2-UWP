using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class HighlightsAndShadowsPage : AdjustmentPage
    {

        public HighlightsAndShadowsAdjustment HighlightsAndShadowsAdjustment;

        public HighlightsAndShadowsPage()
        {
            base.Type = AdjustmentType.HighlightsAndShadows;
            base.Icon = new HighlightsAndShadowsControl();
            this.InitializeComponent();

            this.ShadowsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Shadows = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.HighlightsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Highlights = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.ClaritySlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Clarity = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.MaskBlurAmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount = (float)(value / 10);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();
        public override Adjustment GetAdjustment() => this.HighlightsAndShadowsAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is HighlightsAndShadowsAdjustment adjustment)
            {
                this.HighlightsAndShadowsAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.HighlightsAndShadowsAdjustment = null;
        public override void Reset()
        {
            if (this.HighlightsAndShadowsAdjustment == null) return;

            this.HighlightsAndShadowsAdjustment.Reset();
            this.Invalidate(this.HighlightsAndShadowsAdjustment);
        }

        public void Invalidate(HighlightsAndShadowsAdjustment adjustment)
        {
            this.ShadowsSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Shadows * 100;
            this.HighlightsSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Highlights * 100;
            this.ClaritySlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Clarity * 100;
            this.MaskBlurAmountSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount * 10;
        }
    }
}


