using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "TemperatureAdjustment"/>.
    /// </summary>
    public sealed partial class TemperaturePage : IAdjustmentPage
    {
        public TemperatureAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Temperature;
        public FrameworkElement Icon { get; } = new TemperatureIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public TemperaturePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TemperatureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Temperature = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.TintSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Tint = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new TemperatureAdjustment();

        public void Follow(TemperatureAdjustment adjustment)
        {
            this.TemperatureSlider.Value = adjustment.Temperature * 100;
            this.TintSlider.Value = adjustment.Tint * 100;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Temperature");

            this.TemperatureTextBlock.Text = resource.GetString("/Adjustments/Temperature_Temperature");
            this.TintTextBlock.Text = resource.GetString("/Adjustments/Temperature_Tint");
        }

    }
}

