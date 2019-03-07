using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class SaturationPage : Page
    {
        #region DependencyProperty

        public SaturationAdjustment SaturationAdjustment
        {
            get { return (SaturationAdjustment)GetValue(SaturationAdjustmentProperty); }
            set { SetValue(SaturationAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty SaturationAdjustmentProperty = DependencyProperty.Register(nameof(SaturationAdjustment), typeof(SaturationAdjustment), typeof(SaturationAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            SaturationPage con = (SaturationPage)sender;

            if (e.NewValue is SaturationAdjustment adjustment)
            {
                con.SaturationSlider.Value = adjustment.SaturationAdjustmentItem.Saturation * 100;
            }
        }));

        #endregion


        public SaturationPage()
        {
            this.InitializeComponent();
        }

        private void SaturationSlider_ValueChangeDelta(object sender, double value)
        {
            this.SaturationAdjustment.SaturationAdjustmentItem.Saturation = (float)(value / 100);
            Adjustment.Invalidate();
        }
    }
}
