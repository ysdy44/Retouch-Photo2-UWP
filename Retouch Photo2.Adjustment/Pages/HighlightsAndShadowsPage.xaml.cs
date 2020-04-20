using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentPage
    {
        public HighlightsAndShadowsAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.HighlightsAndShadows;
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }

        //@Construct
        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ShadowsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Shadows = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.HighlightsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Highlights = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.ClaritySlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.Clarity = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.MaskBlurAmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.MaskBlurAmount = (float)(value / 10);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }


        public IAdjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();

        public void Follow(HighlightsAndShadowsAdjustment adjustment)
        {
            this.ShadowsSlider.Value = adjustment.Shadows * 100;
            this.HighlightsSlider.Value = adjustment.Highlights * 100;
            this.ClaritySlider.Value = adjustment.Clarity * 100;
            this.MaskBlurAmountSlider.Value = adjustment.MaskBlurAmount * 10;
        }

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/HighlightsAndShadows");

            this.ShadowsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Shadows");
            this.HighlightsTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Highlights");
            this.ClarityTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_Clarity");
            this.MaskBlurAmountTextBlock.Text = resource.GetString("/Adjustments/HighlightsAndShadows_MaskBlurAmount");
        }

    }
}