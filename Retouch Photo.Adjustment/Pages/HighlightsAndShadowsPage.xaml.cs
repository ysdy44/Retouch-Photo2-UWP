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
                con.ShadowsSlider.Value = adjustment.Shadows * 100;
                con.HighlightsSlider.Value = adjustment.Highlights * 100;
                con.ClaritySlider.Value = adjustment.Clarity * 100;
                con.MaskBlurAmountSlider.Value = adjustment.MaskBlurAmount * 10;
            }
        }));

        #endregion


        public HighlightsAndShadowsPage()
        {
            this.InitializeComponent();
        } 

        private void ShadowsSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.Shadows = (float)(value / 100);
            Adjustment.Invalidate();
        }
        private void HighlightsSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.Highlights = (float)(value / 100);
            Adjustment.Invalidate();
        }
        private void ClaritySlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.Clarity = (float)(value / 100);
            Adjustment.Invalidate();
        }
        private void MaskBlurAmountSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.MaskBlurAmount = (float)(value / 10);
            Adjustment.Invalidate();
        }
    }
}

 
