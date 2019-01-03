using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class ExposurePage : Page
    {
        
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public ExposureAdjustment ExposureAdjustment
        {
            get { return (ExposureAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }        
        public static readonly DependencyProperty MyPropertyProperty =DependencyProperty.Register(nameof(ExposureAdjustment), typeof(ExposureAdjustment), typeof(ExposureAdjustment), new PropertyMetadata(null,(sender,e)=>
        {
            ExposurePage con = (ExposurePage)sender;

            if (e.NewValue is ExposureAdjustment adjustment)
            {
                con.ExposureSlider.Value = adjustment.Exposure * 100;
            }
        }));
        
        #endregion


        public ExposurePage()
        {
            this.InitializeComponent();
        }
         
        private void ExposureSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.ExposureAdjustment.Exposure = (float)(e.NewValue / 100);
            this.ViewModel.Invalidate();
        }
    }
}
