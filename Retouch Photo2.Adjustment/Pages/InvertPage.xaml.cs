using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class InvertPage : AdjustmentPage
    {

        public InvertAdjustment InvertAdjustment;

        public InvertPage()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            this.InitializeComponent();
        }

        //@override
        public override Adjustment GetNewAdjustment() => new InvertAdjustment();
        public override Adjustment GetAdjustment() => this.InvertAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is InvertAdjustment adjustment)
            {
                this.InvertAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.InvertAdjustment = null;
        public override void Reset()
        {
            if (this.InvertAdjustment == null) return;

            this.InvertAdjustment.Item.Reset();
            this.Invalidate(this.InvertAdjustment);
        }

        public void Invalidate(InvertAdjustment adjustment)
        {
        }
    }
}
