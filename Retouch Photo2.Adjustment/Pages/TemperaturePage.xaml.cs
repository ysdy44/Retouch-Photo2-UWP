using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentPage
    {

        public TemperatureAdjustment TemperatureAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public FrameworkElement Page => this;
               
        //@Construct
        public TemperaturePage()
        {
            this.InitializeComponent();

            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.TemperatureAdjustment == null) return;
                this.TemperatureAdjustment.Temperature = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.TemperatureAdjustment == null) return;
                this.TemperatureAdjustment.Tint = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new TemperatureAdjustment();
        public void SetAdjustment(IAdjustment value)
        {
            if (value is TemperatureAdjustment adjustment)
            {
                this.TemperatureAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.TemperatureAdjustment = null;
        public void Reset()
        {
            if (this.TemperatureAdjustment == null) return;

            this.TemperatureAdjustment.Reset();
            this.Invalidate(this.TemperatureAdjustment);
        }

        public void Invalidate(TemperatureAdjustment adjustment)
        {
            this.TemperatureSlider.Value = adjustment.Temperature * 100;
            this.TintSlider.Value = adjustment.Tint * 100;
        }
    }
}

