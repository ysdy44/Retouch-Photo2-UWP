using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class TemperaturePage : Page
    {
        #region DependencyProperty

        public TemperatureAdjustment TemperatureAdjustment
        {
            get { return (TemperatureAdjustment)GetValue(TemperatureAdjustmentProperty); }
            set { SetValue(TemperatureAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty TemperatureAdjustmentProperty = DependencyProperty.Register(nameof(TemperatureAdjustment), typeof(TemperatureAdjustment), typeof(TemperatureAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            TemperaturePage con = (TemperaturePage)sender;

            if(e.NewValue is TemperatureAdjustment adjustment)
            {
                con.TemperatureSlider.Value = adjustment.Temperature * 100;
                con.TintSlider.Value = adjustment.Tint * 100;
            }
        }));

        #endregion


        public TemperaturePage()
        {
            this.InitializeComponent();
        }

        private void TemperatureSlider_ValueChangeDelta(object sender, double value)
        {
            this.TemperatureAdjustment.Temperature = (float)(value / 100);
            Adjustment.Invalidate();
        }

        private void TintSlider_ValueChangeDelta(object sender, double value)
        {
            this.TemperatureAdjustment.Tint = (float)(value / 100);
            Adjustment.Invalidate();
        }
    }
}

