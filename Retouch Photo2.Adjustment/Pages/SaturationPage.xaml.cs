using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentPage
    {

        public SaturationAdjustment SaturationAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationControl();
        public FrameworkElement Page => this;
               
        //@Construct
        public SaturationPage()
        {
            this.InitializeComponent();

            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SaturationAdjustment == null) return;
                this.SaturationAdjustment.Saturation = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        //@override
        public IAdjustment GetNewAdjustment() => new SaturationAdjustment();
        public IAdjustment GetAdjustment() => this.SaturationAdjustment;
        public void SetAdjustment(IAdjustment value)
        {
            if (value is SaturationAdjustment adjustment)
            {
                this.SaturationAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.SaturationAdjustment = null;
        public void Reset()
        {
            if (this.SaturationAdjustment == null) return;

            this.SaturationAdjustment.Reset();
            this.Invalidate(this.SaturationAdjustment);
        }

        public void Invalidate(SaturationAdjustment adjustment)
        {
            this.SaturationSlider.Value = adjustment.Saturation * 100;
        }
    }
}
