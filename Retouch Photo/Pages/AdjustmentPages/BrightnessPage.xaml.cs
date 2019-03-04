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
    public sealed partial class BrightnessPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty


        public BrightnessAdjustment BrightnessAdjustment
        {
            get { return (BrightnessAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(BrightnessAdjustment), typeof(BrightnessAdjustment), typeof(BrightnessAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            BrightnessPage con = (BrightnessPage)sender;

            if (e.NewValue is BrightnessAdjustment adjustment)
            {
                con.WhiteLightSlider.Value = adjustment.WhiteLight * 100;
                con.WhiteDarkSlider.Value = adjustment.WhiteDark * 100;
                con.BlackLightSlider.Value = adjustment.BlackLight * 100;
                con.BlackDarkSlider.Value = adjustment.BlackDark * 100;
            }
        }));


        #endregion


        public BrightnessPage()
        {
            this.InitializeComponent();
        }

        private void WhiteLightSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.WhiteLight = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
        private void WhiteDarkSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.WhiteDark = (float)(value / 100);
            this.ViewModel.Invalidate();
        }

        private void BlackLightSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BlackLight = (float)(value / 100);
            this.ViewModel.Invalidate();
        }
        private void BlackDarkSlider_ValueChangeDelta(object sender, double value)
        {
            this.BrightnessAdjustment.BlackDark = (float)(value / 100);
            this.ViewModel.Invalidate();
        }

    }
}
