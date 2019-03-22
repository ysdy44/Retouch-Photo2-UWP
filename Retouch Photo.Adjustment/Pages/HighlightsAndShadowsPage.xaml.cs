using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class HighlightsAndShadowsPage : Page
    {
        #region DependencyProperty

        public HighlightsAndShadowsAdjustment HighlightsAndShadowsAdjustment
        {
            get { return (HighlightsAndShadowsAdjustment)GetValue(HighlightsAndShadowsAdjustmentProperty); }
            set { SetValue(HighlightsAndShadowsAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty HighlightsAndShadowsAdjustmentProperty = DependencyProperty.Register(nameof(HighlightsAndShadowsAdjustment), typeof(HighlightsAndShadowsAdjustment), typeof(HighlightsAndShadowsAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            HighlightsAndShadowsPage con = (HighlightsAndShadowsPage)sender;

            if(e.NewValue is HighlightsAndShadowsAdjustment adjustment)
            {
                con.ShadowsSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Shadows * 100;
                con.HighlightsSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Highlights * 100;
                con.ClaritySlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.Clarity * 100;
                con.MaskBlurAmountSlider.Value = adjustment.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount * 10;
            }
        }));

        #endregion


        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();
        } 

        private void ShadowsSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Shadows = (float)(value / 100);
            Adjustment.Invalidate?.Invoke();
        }
        private void HighlightsSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Highlights = (float)(value / 100);
            Adjustment.Invalidate?.Invoke();
        }
        private void ClaritySlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.Clarity = (float)(value / 100);
            Adjustment.Invalidate?.Invoke();
        }
        private void MaskBlurAmountSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount = (float)(value / 10);
            Adjustment.Invalidate?.Invoke();
        }
    }
}

 
