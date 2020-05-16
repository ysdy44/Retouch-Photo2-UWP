using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "SaturationAdjustment"/>.
    /// </summary>
    public sealed partial class SaturationPage : IAdjustmentPage
    {
        public SaturationAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Saturation;
        public FrameworkElement Icon { get; } = new SaturationIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public SaturationPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.SaturationSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Saturation = (float)(value / 100);
                Filter.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new SaturationAdjustment();

        public void Follow(SaturationAdjustment adjustment)
        {
            this.SaturationSlider.Value = adjustment.Saturation * 100;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Saturation");

            this.SaturationTextBlock.Text = resource.GetString("/Adjustments/Saturation_Saturation");
        }

    }
}

