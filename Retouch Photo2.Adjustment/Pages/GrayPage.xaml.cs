using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GrayAdjustment"/>.
    /// </summary>
    public sealed partial class GrayPage : IAdjustmentPage
    {

        public GrayAdjustment GrayAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Gray;
        public FrameworkElement Icon { get; } = new GrayControl();
        public FrameworkElement Page => this;
               
        //@Construct
        public GrayPage()
        {
            this.InitializeComponent();            
        }

        
        public IAdjustment GetNewAdjustment() => new GrayAdjustment();
        public IAdjustment GetAdjustment() => this.GrayAdjustment;
        public void SetAdjustment(IAdjustment value)
        {
            if (value is GrayAdjustment adjustment)
            {
                this.GrayAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.GrayAdjustment = null;
        public void Reset()
        {
            if (this.GrayAdjustment == null) return;

            this.GrayAdjustment.Reset();
            this.Invalidate(this.GrayAdjustment);
        }

        public void Invalidate(GrayAdjustment adjustment)
        {
        }
    }
}

