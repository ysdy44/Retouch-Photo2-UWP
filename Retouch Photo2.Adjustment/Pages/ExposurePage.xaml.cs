using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentPage
    {

        public ExposureAdjustment ExposureAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon();
        public FrameworkElement Page => this;

        //@Construct
        public ExposurePage()
        {
            this.InitializeComponent();

            this.ExposureSlider.ValueChangeDelta+=(s, value)=>
            {
                if (this.ExposureAdjustment == null) return;
                this.ExposureAdjustment.Exposure = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new ExposureAdjustment();
        public void SetAdjustment(IAdjustment value)
        {
            if (value is ExposureAdjustment adjustment)
            {
                this.ExposureAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }
        
        public void Close() => this.ExposureAdjustment = null;
        public void Reset()
        {
            if (this.ExposureAdjustment == null) return;

            this.ExposureAdjustment.Reset();
            this.Invalidate(this.ExposureAdjustment);
        }

        public void Invalidate(ExposureAdjustment adjustment)
        {
            this.ExposureSlider.Value = adjustment.Exposure * 100;
        }
    }
}
