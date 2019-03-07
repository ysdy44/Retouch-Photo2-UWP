using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class BrightnessPage : Page
    {

        #region DependencyProperty


        public BrightnessAdjustment BrightnessAdjustment
        {
            get { return (BrightnessAdjustment)GetValue(BrightnessAdjustmentProperty); }
            set { SetValue(BrightnessAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty BrightnessAdjustmentProperty = DependencyProperty.Register(nameof(BrightnessAdjustment), typeof(BrightnessAdjustment), typeof(BrightnessAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            BrightnessPage con = (BrightnessPage)sender;

            if (e.NewValue is BrightnessAdjustment adjustment)
            {
                con.WhiteLightSlider.Value = adjustment.BrightnessAdjustmentItem.WhiteLight * 100;
                con.WhiteDarkSlider.Value = adjustment.BrightnessAdjustmentItem.WhiteDark * 100;
                con.BlackLightSlider.Value = adjustment.BrightnessAdjustmentItem.BlackLight * 100;
                con.BlackDarkSlider.Value = adjustment.BrightnessAdjustmentItem.BlackDark * 100;
            }
        }));


        #endregion


        public BrightnessPage()
        {
            this.InitializeComponent();
        }

        private void WhiteLightSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BrightnessAdjustmentItem.WhiteLight = (float)(value / 100);
            Adjustment.Invalidate();
        }
        private void WhiteDarkSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BrightnessAdjustmentItem.WhiteDark = (float)(value / 100);
            Adjustment.Invalidate();
        }

        private void BlackLightSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BrightnessAdjustmentItem.BlackLight = (float)(value / 100);
            Adjustment.Invalidate();
        }
        private void BlackDarkSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BrightnessAdjustmentItem.BlackDark = (float)(value / 100);
            Adjustment.Invalidate();
        }

    }
}
