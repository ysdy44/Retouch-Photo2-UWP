using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class VignettePage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public VignetteAdjustment VignetteAdjustment
        {
            get { return (VignetteAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(VignetteAdjustment), typeof(VignetteAdjustment), typeof(VignetteAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            VignettePage con = (VignettePage)sender;

            if (e.NewValue is VignetteAdjustment adjustment)
            {
                con.AmountSlider.Value = adjustment.Amount * 100;
                con.CurveSlider.Value = adjustment.Curve * 100;

                con.SolidColorBrush.Color =
                con.AmountRight.Color =
                con.CurveRight.Color =
                adjustment.Color;
            }
        }));

        #endregion


        public VignettePage()
        {
            this.InitializeComponent();
        }

        private void AmountSlider_ValueChangeDelta(object sender, double value)
        {
            this.VignetteAdjustment.Amount = (float)(value / 100);
            this.ViewModel.Invalidate();
        }

        private void CurveSlider_ValueChangeDelta(object sender, double value)
        {
            this.VignetteAdjustment.Curve = (float)(value / 100);
            this.ViewModel.Invalidate();
        }

        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
            this.ColorPicker.Color = this.ViewModel.Color;
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.SolidColorBrush.Color =
            this.AmountRight.Color =
            this.CurveRight.Color = value;

            if (this.VignetteAdjustment == null) return;

            this.VignetteAdjustment.Color = value;
            this.ViewModel.Invalidate();
        }
        
    }
}

