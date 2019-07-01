using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : AdjustmentPage
    {

        public GrayAdjustment GrayAdjustment;

        //@Construct
        public GrayPage()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new GrayControl();
            this.InitializeComponent();            
        }

        //@override
        public override Adjustment GetNewAdjustment() => new GrayAdjustment();
        public override Adjustment GetAdjustment() => this.GrayAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is GrayAdjustment adjustment)
            {
                this.GrayAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.GrayAdjustment = null;
        public override void Reset()
        {
            if (this.GrayAdjustment == null) return;

            this.GrayAdjustment.Item.Reset();
            this.Invalidate(this.GrayAdjustment);
        }

        public void Invalidate(GrayAdjustment adjustment)
        {
        }
    }
}

