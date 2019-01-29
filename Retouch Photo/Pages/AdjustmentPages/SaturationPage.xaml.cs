using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class SaturationPage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public SaturationAdjustment SaturationAdjustment
        {
            get { return (SaturationAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(SaturationAdjustment), typeof(SaturationAdjustment), typeof(SaturationAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            SaturationPage con = (SaturationPage)sender;

            if (e.NewValue is SaturationAdjustment adjustment)
            {
                con.SaturationSlider.Value = adjustment.Saturation * 100;
            }
        }));

        #endregion


        public SaturationPage()
        {
            this.InitializeComponent();
        }

        private void SaturationSlider_ValueChangeDelta(object sender, double value)
        {
            this.SaturationAdjustment.Saturation = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
    }
}
