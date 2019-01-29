using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class TemperaturePage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public TemperatureAdjustment TemperatureAdjustment
        {
            get { return (TemperatureAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(TemperatureAdjustment), typeof(TemperatureAdjustment), typeof(TemperatureAdjustment), new PropertyMetadata(null, (sender, e) =>
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
            this.ViewModel.Invalidate();
        }

        private void TintSlider_ValueChangeDelta(object sender, double value)
        {
            this.TemperatureAdjustment.Tint = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
    }
}

