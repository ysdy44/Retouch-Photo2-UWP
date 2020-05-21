using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "VignetteAdjustment"/>.
    /// </summary>
    public sealed partial class VignettePage : IAdjustmentPage
    {
        public VignetteAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Vignette;
        public FrameworkElement Icon { get; } = new VignetteIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public VignettePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Amount = (float)(value / 100);
                Filter.Invalidate?.Invoke();
            };
            this.CurveSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Curve = (float)(value / 100);
                Filter.Invalidate?.Invoke();
            };
            this.ColorBorder.Tapped += (s, e) =>
            {
                if (this.Adjustment == null) return;
                this.ColorFlyout.ShowAt(this.ColorBorder);
                this.ColorPicker.Color = this.Adjustment.Color;
            };
            //TODO
            this.ColorPicker.ColorChanged += (s, value) =>
            {
                this.SolidColorBrush.Color =
                this.AmountRight.Color =
                this.CurveRight.Color = value;

                if (this.Adjustment == null) return;

                this.Adjustment.Color = value;
                Filter.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new VignetteAdjustment();

        public void Follow(VignetteAdjustment adjustment)
        {
            this.AmountSlider.Value = adjustment.Amount * 100;
            this.CurveSlider.Value = adjustment.Curve * 100;
            this.SolidColorBrush.Color = this.AmountRight.Color = this.CurveRight.Color = adjustment.Color;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Vignette");

            this.AmountTextBlock.Text = resource.GetString("/Adjustments/Vignette_Amount");
            this.CurveTextBlock.Text = resource.GetString("/Adjustments/Vignette_Curve");
            this.ColorTextBlock.Text = resource.GetString("/Adjustments/Vignette_Color");
        }

    }
}

