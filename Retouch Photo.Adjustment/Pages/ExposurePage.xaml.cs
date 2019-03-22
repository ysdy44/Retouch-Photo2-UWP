using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class ExposurePage : Page
    {
        #region DependencyProperty

        public ExposureAdjustment ExposureAdjustment
        {
            get { return (ExposureAdjustment)GetValue(ExposureAdjustmentProperty); }
            set { SetValue(ExposureAdjustmentProperty, value); }
        }        
        public static readonly DependencyProperty ExposureAdjustmentProperty =DependencyProperty.Register(nameof(ExposureAdjustment), typeof(ExposureAdjustment), typeof(ExposureAdjustment), new PropertyMetadata(null,(sender,e)=>
        {
            ExposurePage con = (ExposurePage)sender;

            if(e.NewValue is ExposureAdjustment adjustment)
            {
                con.ExposureSlider.Value = adjustment.ExposureAdjustmentItem.Exposure * 100;
            }
        }));
        
        #endregion


        public ExposurePage()
        {
            this.InitializeComponent();
        }
         
        private void ExposureSlider_ValueChangeDelta(object sender, double value)
        {
            this.ExposureAdjustment.ExposureAdjustmentItem.Exposure = (float)(value / 100);
            Adjustment.Invalidate?.Invoke();
        }
    }
}
