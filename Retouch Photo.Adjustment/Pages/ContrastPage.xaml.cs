using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class ContrastPage : AdjustmentPage
    {

        public ContrastAdjustment ContrastAdjustment;

        public ContrastPage()
        {
            base.Type = AdjustmentType.Contrast;
            base.Icon = new ContrastControl();
            this.InitializeComponent();

            this.ContrastSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.ContrastAdjustment == null) return;
                this.ContrastAdjustment.ContrastAdjustmentItem.Contrast = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new ContrastAdjustment();
        public override Adjustment GetAdjustment() => this.ContrastAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is ContrastAdjustment adjustment)
            {
                this.ContrastAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.ContrastAdjustment = null;
        public override void Reset()
        {
            if (this.ContrastAdjustment == null) return;

            this.ContrastAdjustment.Reset();
            this.Invalidate(this.ContrastAdjustment);
        }

        public void Invalidate(ContrastAdjustment adjustment)
        {
            this.ContrastSlider.Value = adjustment.ContrastAdjustmentItem.Contrast * 100;
        }
    }
}

