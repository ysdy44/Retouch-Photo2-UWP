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
    public sealed partial class StraightenPage : Page
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
            StraightenPage con = (StraightenPage)sender;

            if (e.NewValue is EffectManager effectManager)
            {
                StraightenEffectItem item = effectManager.StraightenEffectItem;

                con.AnglePicker.Radians = item.Angle * 4.0f;
            }
        }));

        #endregion


        public StraightenPage()
        {
            this.InitializeComponent();
        }

        private void AnglePicker_AngleChange(float radians)
        {
            if (this.EffectManager == null) return;

            this.EffectManager.StraightenEffectItem.Angle = radians / 4.0f;
            this.ViewModel.Invalidate();
        }
    }
}
