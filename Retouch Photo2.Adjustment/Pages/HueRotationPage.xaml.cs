using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using System;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class HueRotationPage : AdjustmentPage
    {

        public HueRotationAdjustment HueRotationAdjustment;

        public HueRotationPage()
        {
            base.Type = AdjustmentType.HueRotation;
            base.Icon = new HueRotationControl();
            this.InitializeComponent();

            this.HueRotationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HueRotationAdjustment == null) return;
                this.HueRotationAdjustment.HueRotationAdjustmentitem.Angle = (float)(value * Math.PI / 180);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new HueRotationAdjustment();
        public override Adjustment GetAdjustment() => this.HueRotationAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is HueRotationAdjustment adjustment)
            {
                this.HueRotationAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.HueRotationAdjustment = null;
        public override void Reset()
        {
            if (this.HueRotationAdjustment == null) return;

            this.HueRotationAdjustment.Item.Reset();
            this.Invalidate(this.HueRotationAdjustment);
        }

        public void Invalidate(HueRotationAdjustment adjustment)
        {
            this.HueRotationSlider.Value = adjustment.HueRotationAdjustmentitem.Angle * 180 / Math.PI;
        }
    }
}

