using Retouch_Photo.Adjustments.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class HueRotationPage : Page
    {
        #region DependencyProperty

        public HueRotationAdjustment HueRotationAdjustment
        {
            get { return (HueRotationAdjustment)GetValue(HueRotationAdjustmentProperty); }
            set { SetValue(HueRotationAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty HueRotationAdjustmentProperty = DependencyProperty.Register(nameof(HueRotationAdjustment), typeof(HueRotationAdjustment), typeof(HueRotationAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            HueRotationPage con = (HueRotationPage)sender;

            if(e.NewValue is HueRotationAdjustment adjustment)
            {
                con.HueRotationSlider.Value = adjustment.HueRotationAdjustmentitem.Angle * 180/Math.PI;
            }
        }));

        #endregion


        public HueRotationPage()
        {
            this.InitializeComponent();
        }

        private void HueRotationSlider_ValueChangeDelta(object sender, double value)
        {
            this.HueRotationAdjustment.HueRotationAdjustmentitem.Angle = (float)(value * Math.PI / 180);
            Adjustment.Invalidate?.Invoke();
        }
    }
}

