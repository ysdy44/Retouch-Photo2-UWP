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

        GaussianBlurEffect GaussianBlurEffect = new GaussianBlurEffect();

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
                con.GaussianBlurEffect.Open(layer.EffectManager.GaussianBlurEffectIsOn);
            }
            else
            {
                con.GaussianBlurEffect.Close();
            }
        }));

        #endregion
   
        public EffectsControl()
        {
            this.InitializeComponent();            

            this.ItemsControl.ItemsSource = new List<Effect>
            {
                this.GaussianBlurEffect
            };

            this.BackButton.Tapped += (sender, e) => this.Clear();
            this.ResetButton.Tapped += (sender, e) => this.Reset();
        }




        private void EffectControl_EffectToggle(Effect effect)
        {
            if (this.Layer == null) return;

            effect.Set(this.Layer.EffectManager);

            this.ViewModel.Invalidate();
        }
        private void EffectControl_EffectTapped(Effect effect)
        {
            this.Effect = effect;

            if (this.Layer == null) return;

            effect.SetPage(this.Layer.EffectManager);
            this.EffectContextFrame.Child = effect.Page;

            this.EffectContextControl.Visibility = Visibility.Visible;
        }





        /// <summary> 重置 </summary>
        private void Reset()
        {
            if (this.Effect == null) return;

            this.Effect.Reset(this.Layer.EffectManager); 

            this.ViewModel.Invalidate();
        }
        /// <summary> 清空 </summary>
        private void Clear()
        {
            this.Effect = null;
            this.EffectContextFrame.Child = null;
            this.EffectContextControl.Visibility = Visibility.Collapsed; 
        }

    }
}
