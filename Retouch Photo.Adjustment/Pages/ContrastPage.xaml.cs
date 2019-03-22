using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class ContrastPage : Page
    {
        #region DependencyProperty

        public ContrastAdjustment ContrastAdjustment
        {
            get { return (ContrastAdjustment)GetValue(ContrastAdjustmentProperty); }
            set { SetValue(ContrastAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty ContrastAdjustmentProperty = DependencyProperty.Register(nameof(ContrastAdjustment), typeof(ContrastAdjustment), typeof(ContrastAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            ContrastPage con = (ContrastPage)sender;

            if(e.NewValue is ContrastAdjustment adjustment)
            {
                con.ContrastSlider.Value = adjustment.ContrastAdjustmentItem.Contrast * 100;
            }
        }));

        #endregion


        public ContrastPage()
        {
            this.InitializeComponent();
        }

        private void ContrastSlider_ValueChangeDelta(object sender, double value)
        {
            this.ContrastAdjustment.ContrastAdjustmentItem.Contrast = (float)(value / 100);
            Adjustment.Invalidate?.Invoke();
        }
    }
}

