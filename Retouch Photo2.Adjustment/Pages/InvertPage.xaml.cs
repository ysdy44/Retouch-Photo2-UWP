using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "InvertAdjustment"/>.
    /// </summary>
    public sealed partial class InvertPage : IAdjustmentPage
    {

        public InvertAdjustment InvertAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Invert;
        public FrameworkElement Icon { get; } = new InvertControl();
        public FrameworkElement Page => this;
               
        //@Construct
        public InvertPage()
        {
            this.InitializeComponent();
        }

        //@override
        public IAdjustment GetNewAdjustment() => new InvertAdjustment();
        public IAdjustment GetAdjustment() => this.InvertAdjustment;
        public void SetAdjustment(IAdjustment value)
        {
            if (value is InvertAdjustment adjustment)
            {
                this.InvertAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.InvertAdjustment = null;
        public void Reset()
        {
            if (this.InvertAdjustment == null) return;

            this.InvertAdjustment.Reset();
            this.Invalidate(this.InvertAdjustment);
        }

        public void Invalidate(InvertAdjustment adjustment)
        {
        }
    }
}
