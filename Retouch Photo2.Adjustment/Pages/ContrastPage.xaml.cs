using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentPage
    {

        public ContrastAdjustment ContrastAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastControl();
        public FrameworkElement Page => this;

        //@Construct
        public ContrastPage()
        {
            this.InitializeComponent();

            this.ContrastSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.ContrastAdjustment == null) return;
                this.ContrastAdjustment.Contrast = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        //@override
        public IAdjustment GetNewAdjustment() => new ContrastAdjustment();
        public IAdjustment GetAdjustment() => this.ContrastAdjustment;
        public void SetAdjustment(IAdjustment value)
        {
            if (value is ContrastAdjustment adjustment)
            {
                this.ContrastAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.ContrastAdjustment = null;
        public void Reset()
        {
            if (this.ContrastAdjustment == null) return;

            this.ContrastAdjustment.Reset();
            this.Invalidate(this.ContrastAdjustment);
        }

        public void Invalidate(ContrastAdjustment adjustment)
        {
            this.ContrastSlider.Value = adjustment.Contrast * 100;
        }
    }
}

