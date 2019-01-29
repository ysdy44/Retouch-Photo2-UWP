using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class HighlightsAndShadowsPage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public HighlightsAndShadowsAdjustment HighlightsAndShadowsAdjustment
        {
            get { return (HighlightsAndShadowsAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(HighlightsAndShadowsAdjustment), typeof(HighlightsAndShadowsAdjustment), typeof(HighlightsAndShadowsAdjustment), new PropertyMetadata(null, (sender, e) =>
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
            this.ViewModel.Invalidate();
        }
        private void HighlightsSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.Highlights = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
        private void ClaritySlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.Clarity = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
        private void MaskBlurAmountSlider_ValueChangeDelta(object sender, double value)
        {
            this.HighlightsAndShadowsAdjustment.MaskBlurAmount = (float)(value / 10);
            this.ViewModel.Invalidate();
        }
    }
}

 
