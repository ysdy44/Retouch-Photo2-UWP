using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentPage
    {
        public BrightnessAdjustment BrightnessAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessControl();
        public FrameworkElement Page => this;

        //@Construct
        public BrightnessPage()
        {
            this.InitializeComponent();

            this.WhiteLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.WhiteLight = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.WhiteDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.WhiteDark = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };

            this.BlackLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BlackLight = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlackDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.BrightnessAdjustment == null) return;
                this.BrightnessAdjustment.BlackDark = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new BrightnessAdjustment();
        public IAdjustment GetAdjustment() => this.BrightnessAdjustment;
        public void SetAdjustment(IAdjustment value)
        {
            if (value is BrightnessAdjustment adjustment)
            {
                this.BrightnessAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.BrightnessAdjustment = null;
        public void Reset()
        {
            if (this.BrightnessAdjustment == null) return;

            this.BrightnessAdjustment.Reset();
            this.Invalidate(this.BrightnessAdjustment);
        }

        public void Invalidate(BrightnessAdjustment adjustment)
        {
            this.WhiteLightSlider.Value = adjustment.WhiteLight * 100;
            this.WhiteDarkSlider.Value = adjustment.WhiteDark * 100;
            this.BlackLightSlider.Value = adjustment.BlackLight * 100;
            this.BlackDarkSlider.Value = adjustment.BlackDark * 100;
        }
    }
}