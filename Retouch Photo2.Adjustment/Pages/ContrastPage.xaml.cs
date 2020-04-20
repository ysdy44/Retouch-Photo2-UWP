using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "ContrastAdjustment"/>.
    /// </summary>
    public sealed partial class ContrastPage : IAdjustmentPage
    {
        public ContrastAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.Contrast;
        public FrameworkElement Icon { get; } = new ContrastIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public ContrastPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ContrastSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Contrast = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new ContrastAdjustment();
  
        public void Follow(ContrastAdjustment adjustment)
        {
            this.ContrastSlider.Value = adjustment.Contrast * 100;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Contrast");

            this.ContrastTextBlock.Text = resource.GetString("/Adjustments/Contrast_Contrast");
        }

    }
}

