using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ExposureAdjustment"/>.
    /// </summary>
    public sealed partial class ExposurePage : IAdjustmentPage
    {
        public ExposureAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Exposure;
        public FrameworkElement Icon { get; } = new ExposureIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public ExposurePage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ExposureSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Exposure = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new ExposureAdjustment();

        public void Follow(ExposureAdjustment adjustment)
        {
            this.ExposureSlider.Value = adjustment.Exposure * 100;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Exposure");

            this.ExposureTextBlock.Text = resource.GetString("/Adjustments/Exposure_Exposure");
        }

    }
}

