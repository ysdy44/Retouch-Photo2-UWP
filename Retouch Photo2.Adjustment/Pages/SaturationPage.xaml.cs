using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class SaturationPage : AdjustmentPage
    {

        public SaturationAdjustment SaturationAdjustment;

        public SaturationPage()
        {
            base.Type = AdjustmentType.Saturation;
            base.Icon = new SaturationControl();
            this.InitializeComponent();

            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SaturationAdjustment == null) return;
                this.SaturationAdjustment.SaturationAdjustmentItem.Saturation = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new SaturationAdjustment();
        public override Adjustment GetAdjustment() => this.SaturationAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is SaturationAdjustment adjustment)
            {
                this.SaturationAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.SaturationAdjustment = null;
        public override void Reset()
        {
            if (this.SaturationAdjustment == null) return;

            this.SaturationAdjustment.Item.Reset();
            this.Invalidate(this.SaturationAdjustment);
        }

        public void Invalidate(SaturationAdjustment adjustment)
        {
            this.SaturationSlider.Value = adjustment.SaturationAdjustmentItem.Saturation * 100;
        }
    }
}
