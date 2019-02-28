using Retouch_Photo.Element;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Effects;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pages.EffectPages
{
    public sealed partial class OuterShadowPage : Page
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
            OuterShadowPage con = (OuterShadowPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                OuterShadowEffectItem item = effectManager.OuterShadowEffectItem;

                con.RadiusSlider.Value = item.Radius;
                con.OpacitySlider.Value = item.Opacity * 100.0;
                con.OffsetSlider.Value = item.Offset;
                con.AnglePicker.Radians = item.Angle;
                con.SolidColorBrush.Color = item.Color;
            }
        }));

        #endregion


        public OuterShadowPage()
        {
            this.InitializeComponent();
        }

        private void RadiusSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Radius = (float)e.NewValue;
            this.ViewModel.Invalidate();
        }

        private void OpacitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Opacity = (float)(e.NewValue / 100.0);
            this.ViewModel.Invalidate();
        }


        private void OffsetSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Offset = (float)e.NewValue;
            this.ViewModel.Invalidate();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;
            
            this.EffectManager.OuterShadowEffectItem.Angle = radians;
            this.ViewModel.Invalidate();
        }
         
        private void ColorButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.ColorFlyout.ShowAt(this.ColorButton);
            this.ColorPicker.Color = this.ViewModel.Color;
        }
        private void ColorPicker_ColorChange(object sender, Color value)
        {
            this.SolidColorBrush.Color = value;

            if (this.EffectManager == null) return;

            this.EffectManager.OuterShadowEffectItem.Color = value;
            this.ViewModel.Invalidate();
        }

    }
}
