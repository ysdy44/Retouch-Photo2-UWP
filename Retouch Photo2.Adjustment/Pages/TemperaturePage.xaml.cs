using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : AdjustmentPage
    {

        public TemperatureAdjustment TemperatureAdjustment;

        //@Construct
        public TemperaturePage()
        {
            base.Type = AdjustmentType.Temperature;
            base.Icon = new TemperatureControl();
            this.InitializeComponent();

            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.TemperatureAdjustment == null) return;
                this.TemperatureAdjustment.TemperatureAdjustmentItem.Temperature = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.TemperatureAdjustment == null) return;
                this.TemperatureAdjustment.TemperatureAdjustmentItem.Tint = (float)(value / 100);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new TemperatureAdjustment();
        public override Adjustment GetAdjustment() => this.TemperatureAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is TemperatureAdjustment adjustment)
            {
                this.TemperatureAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.TemperatureAdjustment = null;
        public override void Reset()
        {
            if (this.TemperatureAdjustment == null) return;

            this.TemperatureAdjustment.Item.Reset();
            this.Invalidate(this.TemperatureAdjustment);
        }

        public void Invalidate(TemperatureAdjustment adjustment)
        {
            this.TemperatureSlider.Value = adjustment.TemperatureAdjustmentItem.Temperature * 100;
            this.TintSlider.Value = adjustment.TemperatureAdjustmentItem.Tint * 100;
        }
    }
}

