using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class ExposurePage : AdjustmentPage
    {

        public ExposureAdjustment ExposureAdjustment;

        public ExposurePage()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            this.InitializeComponent();

            this.ExposureSlider.ValueChangeDelta+=(s, value)=>
            {
                if (this.ExposureAdjustment == null) return;
                this.ExposureAdjustment.ExposureAdjustmentItem.Exposure = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new ExposureAdjustment();
        public override Adjustment GetAdjustment() => this.ExposureAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is ExposureAdjustment adjustment)
            {
                this.ExposureAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }
        
        public override void Close() => this.ExposureAdjustment = null;
        public override void Reset()
        {
            if (this.ExposureAdjustment == null) return;

            this.ExposureAdjustment.Item.Reset();
            this.Invalidate(this.ExposureAdjustment);
        }

        public void Invalidate(ExposureAdjustment adjustment)
        {
            this.ExposureSlider.Value = adjustment.ExposureAdjustmentItem.Exposure * 100;
        }
    }
}
