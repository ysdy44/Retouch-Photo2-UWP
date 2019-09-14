using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "HighlightsAndShadowsAdjustment"/>.
    /// </summary>
    public sealed partial class HighlightsAndShadowsPage : IAdjustmentPage
    {

        public HighlightsAndShadowsAdjustment HighlightsAndShadowsAdjustment;

        public AdjustmentType Type { get; } = AdjustmentType.HighlightsAndShadows;
        public FrameworkElement Icon { get; } = new HighlightsAndShadowsIcon();
        public FrameworkElement Page => this;
               
        //@Construct
        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();

            this.ShadowsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.Shadows = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.HighlightsSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.Highlights = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.ClaritySlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.Clarity = (float)(value / 100);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.MaskBlurAmountSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.HighlightsAndShadowsAdjustment == null) return;
                this.HighlightsAndShadowsAdjustment.MaskBlurAmount = (float)(value / 10);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();
        public void SetAdjustment(IAdjustment value)
        {
            if (value is HighlightsAndShadowsAdjustment adjustment)
            {
                this.HighlightsAndShadowsAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public void Close() => this.HighlightsAndShadowsAdjustment = null;
        public void Reset()
        {
            if (this.HighlightsAndShadowsAdjustment == null) return;

            this.HighlightsAndShadowsAdjustment.Reset();
            this.Invalidate(this.HighlightsAndShadowsAdjustment);
        }

        public void Invalidate(HighlightsAndShadowsAdjustment adjustment)
        {
            this.ShadowsSlider.Value = adjustment.Shadows * 100;
            this.HighlightsSlider.Value = adjustment.Highlights * 100;
            this.ClaritySlider.Value = adjustment.Clarity * 100;
            this.MaskBlurAmountSlider.Value = adjustment.MaskBlurAmount * 10;
        }
    }
}


