using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class BrightnessPage : AdjustmentPage
    {
        public BrightnessAdjustment BrightnessAdjustment;
        
        public BrightnessPage()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            this.InitializeComponent();

            this.WhiteLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BrightnessAdjustmentItem.WhiteLight = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.WhiteDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BrightnessAdjustmentItem.WhiteDark = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };

            this.BlackLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BrightnessAdjustmentItem.BlackLight = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.BlackDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BrightnessAdjustmentItem.BlackDark = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new BrightnessAdjustment();
        public override Adjustment GetAdjustment() => this.BrightnessAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is BrightnessAdjustment adjustment) 
            {
                this.BrightnessAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.BrightnessAdjustment = null;
        public override void Reset()
        {
            if (this.BrightnessAdjustment == null) return;

            this.BrightnessAdjustment.Reset();
            this.Invalidate(this.BrightnessAdjustment);
        }

        public void Invalidate(BrightnessAdjustment adjustment)
        {
            this.WhiteLightSlider.Value = adjustment.BrightnessAdjustmentItem.WhiteLight * 100;
            this.WhiteDarkSlider.Value = adjustment.BrightnessAdjustmentItem.WhiteDark * 100;
            this.BlackLightSlider.Value = adjustment.BrightnessAdjustmentItem.BlackLight * 100;
            this.BlackDarkSlider.Value = adjustment.BrightnessAdjustmentItem.BlackDark * 100;
        }
    }
}
