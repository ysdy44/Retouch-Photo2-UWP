using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentPage
    {

        public VignetteAdjustment VignetteAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public FrameworkElement Page => this;
        
        //@Construct
        public VignettePage()
        {
            this.InitializeComponent();

            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.VignetteAdjustment.Amount = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.VignetteAdjustment.Curve = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.ColorButton.Tapped += (s, e) =>
            {
                if (this.VignetteAdjustment == null) return;
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.VignetteAdjustment.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color =
                this.AmountRight.Color =
                this.CurveRight.Color = value;

                if (this.VignetteAdjustment == null) return;

                this.VignetteAdjustment.Color = value;
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new VignetteAdjustment();
        public void SetAdjustment(IAdjustment value)
        {
            if (value is VignetteAdjustment adjustment)
            {
                this.VignetteAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.VignetteAdjustment = null;
        public void Reset()
        {
            if (this.VignetteAdjustment == null) return;

            this.VignetteAdjustment.Reset();
            this.Invalidate(this.VignetteAdjustment);
        }

        public void Invalidate(VignetteAdjustment adjustment)
        {
            this.AmountSlider.Value = adjustment.Amount * 100;
            this.CurveSlider.Value = adjustment.Curve * 100;
            this.SolidColorBrush.Color = this.AmountRight.Color = this.CurveRight.Color = adjustment.Color;
        }
    }
}

