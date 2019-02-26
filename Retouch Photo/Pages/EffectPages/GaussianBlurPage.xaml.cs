using Retouch_Photo.Models;
using Retouch_Photo.Models.Effects;
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

namespace Retouch_Photo.Pages.EffectPages
{
    public sealed partial class GaussianBlurPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty

        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectManager), new PropertyMetadata(null, (sender, e) =>
        {
            GaussianBlurPage con = (GaussianBlurPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                con.BlurAmountSlider.Value = effectManager.BlurAmount;
            }
        }));

        #endregion


        public GaussianBlurPage()
        {
            this.InitializeComponent();
        }

        private void BlurAmountSlider_ValueChangeDelta(object sender, double value)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.BlurAmount = (float)value;
            this.ViewModel.Invalidate();
        }
    }
}
