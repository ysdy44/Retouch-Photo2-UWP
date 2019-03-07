using Retouch_Photo.Adjustments.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class VignettePage : Page
    {
        #region DependencyProperty

        public VignetteAdjustment VignetteAdjustment
        {
            get { return (VignetteAdjustment)GetValue(VignetteAdjustmentProperty); }
            set { SetValue(VignetteAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty VignetteAdjustmentProperty = DependencyProperty.Register(nameof(VignetteAdjustment), typeof(VignetteAdjustment), typeof(VignetteAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            VignettePage con = (VignettePage)sender;

            if (e.NewValue is VignetteAdjustment adjustment)
            {
                con.AmountSlider.Value = adjustment.VignetteAdjustmentItem.Amount * 100;
                con.CurveSlider.Value = adjustment.VignetteAdjustmentItem.Curve * 100;

                con.SolidColorBrush.Color =
                con.AmountRight.Color =
                con.CurveRight.Color =
                adjustment.VignetteAdjustmentItem.Color;
            }
        }));

        #endregion


        public VignettePage()
        {
            this.InitializeComponent();
        }

        private void AmountSlider_ValueChangeDelta(object sender, double value)
        {
            this.VignetteAdjustment.VignetteAdjustmentItem.Amount = (float)(value / 100);
            Adjustment.Invalidate();
        }

        private void CurveSlider_ValueChangeDelta(object sender, double value)
        {
            this.VignetteAdjustment.VignetteAdjustmentItem.Curve = (float)(value / 100);
            Adjustment.Invalidate();
        }

        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
            this.ColorPicker.Color = this.VignetteAdjustment.VignetteAdjustmentItem.Color;
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.SolidColorBrush.Color =
            this.AmountRight.Color =
            this.CurveRight.Color = value;

            if (this.VignetteAdjustment == null) return;

            this.VignetteAdjustment.VignetteAdjustmentItem.Color = value;
            Adjustment.Invalidate();
        }
        
    }
}

