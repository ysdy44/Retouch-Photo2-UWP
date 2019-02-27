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
                GaussianBlurEffectItem item = effectManager.GaussianBlurEffectItem;

                con.BlurAmountSlider.Value = item.BlurAmount;
            }
        }));

        #endregion


        public GaussianBlurPage()
        {
            this.InitializeComponent();
        }
        
        private void BlurAmountSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.GaussianBlurEffectItem.BlurAmount = (float)e.NewValue;
            this.ViewModel.Invalidate();
        }
    }
}
