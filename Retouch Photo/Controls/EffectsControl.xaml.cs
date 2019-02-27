using Retouch_Photo.Controls.EffectControls;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Effects;
using Retouch_Photo.Pages.EffectPages;
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


namespace Retouch_Photo.Controls
{
    public sealed partial class EffectsControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        
        Effect Effect;
        List<Effect> Effects = new List<Effect>
        {
            new GaussianBlurEffect(),
            new OuterShadowEffect(),
        };

        /// <summary>
        /// 页面的内容：
        ///   - null就页面不可视
        ///   - 否则就页面可视
        /// </summary>
        public UIElement EffectContextFrameChild
        {
            set
            {
                if (value==null)
                {
                    this.EffectContextFrame.Child = null;
                    this.EffectContextControl.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.EffectContextFrame.Child = value;
                    this.EffectContextControl.Visibility = Visibility.Visible;
                }
            }
        }


        #region DependencyProperty


        public Layer Layer
        {
            get { return (Layer)GetValue(LayerProperty); }
            set { SetValue(LayerProperty, value); }
        }
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(Layer), typeof(EffectsControl), new PropertyMetadata(null, (sender, e) =>
        {
            EffectsControl con = (EffectsControl)sender;

            if (e.NewValue is Layer layer)
            {
                if (e.OldValue is Layer oldLayer)
                {
                    con.EffectContextFrameChild = null;
                }

                foreach (var effect in con.Effects)
                {
                    effect.Open(layer.EffectManager);
                }
             }
            else
            {

                foreach (var effect in con.Effects)
                {
                    effect.Close();
                }
            }
        }));

        #endregion
   

        public EffectsControl()
        {
            this.InitializeComponent();

            this.ItemsControl.ItemsSource = this.Effects;

            this.BackButton.Tapped += (sender, e) => this.Clear();
            this.ResetButton.Tapped += (sender, e) => this.Reset();
        }
        


        private void EffectControl_EffectToggle(Effect effect)
        {
            if (this.Layer == null) return;


            bool value = effect.ToggleSwitch.IsOn;
            effect.IsOn = value;

            EffectItem effectItem = effect.GetItem(this.Layer.EffectManager);
            effectItem.IsOn  = value;


            this.ViewModel.Invalidate();
        }
        private void EffectControl_EffectTapped(Effect effect)
        {
            this.Effect = effect;

            if (this.Layer == null) return;

            effect.SetPage(this.Layer.EffectManager);


            this.EffectContextFrameChild = effect.Page;
        }
               


        /// <summary> 重置 </summary>
        private void Reset()
        {
            if (this.Effect == null) return;

            this.Effect.Reset(this.Layer.EffectManager);
            this.Effect.SetPage(this.Layer.EffectManager);

            this.ViewModel.Invalidate();
        }
        /// <summary> 清空 </summary>
        private void Clear()
        {
            this.Effect = null;
            this.EffectContextFrameChild = null;
        }
                     
    }
}
