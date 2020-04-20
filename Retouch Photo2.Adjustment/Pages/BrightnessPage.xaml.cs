using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentPage
    {
        public BrightnessAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public BrightnessPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.WhiteLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.WhiteLight = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.WhiteDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.WhiteDark = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };

            this.BlackLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlackLight = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlackDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlackDark = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new BrightnessAdjustment();

        public void Follow(BrightnessAdjustment adjustment)
        {
            this.WhiteLightSlider.Value = adjustment.WhiteLight * 100;
            this.WhiteDarkSlider.Value = adjustment.WhiteDark * 100;
            this.BlackLightSlider.Value = adjustment.BlackLight * 100;
            this.BlackDarkSlider.Value = adjustment.BlackDark * 100;
        }

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Brightness");

            this.WhiteToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToLight");
            this.WhiteToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToDark");
            this.BlackToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToLight");
            this.BlackToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToDark");
        }

    }
}