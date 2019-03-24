using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class VignettePage : AdjustmentPage
    {

        public VignetteAdjustment VignetteAdjustment;

        public VignettePage()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            this.InitializeComponent();

            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.VignetteAdjustment.VignetteAdjustmentItem.Amount = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.VignetteAdjustment.VignetteAdjustmentItem.Curve = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.VignetteAdjustment.VignetteAdjustmentItem.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color =
                this.AmountRight.Color =
                this.CurveRight.Color = value;

                if (this.VignetteAdjustment == null) return;

                this.VignetteAdjustment.VignetteAdjustmentItem.Color = value;
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new VignetteAdjustment();
        public override Adjustment GetAdjustment() => this.VignetteAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is VignetteAdjustment adjustment)
            {
                this.VignetteAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.VignetteAdjustment = null;
        public override void Reset()
        {
            if (this.VignetteAdjustment == null) return;

            this.VignetteAdjustment.Item.Reset();
            this.Invalidate(this.VignetteAdjustment);
        }

        public void Invalidate(VignetteAdjustment adjustment)
        {
            this.AmountSlider.Value = adjustment.VignetteAdjustmentItem.Amount * 100;
            this.CurveSlider.Value = adjustment.VignetteAdjustmentItem.Curve * 100;
            this.SolidColorBrush.Color = this.AmountRight.Color = this.CurveRight.Color = adjustment.VignetteAdjustmentItem.Color;
        }
    }
}

