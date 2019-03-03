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
                
        public bool FrameVisibility
        {
            set
            {
                this.BackButton.Visibility =
                this.ResetButton.Visibility =
                this.Frame.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                this.ItemsControl.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        
        private Effect effect;
        public Effect Effect
        {
            get => this.effect;
            set
            {
                if (value == null) return;
                if (this.Layer == null) return;

                value.SetPage(this.Layer.EffectManager);

                this.Frame.Child = value.Page;
                this.FrameVisibility = true;

                this.effect = value;
            }
        }

        List<Effect> Effects = new List<Effect>
        {
            new GaussianBlurEffect(),
            new DirectionalBlurEffect(),
            new OuterShadowEffect(),

            new OutlineEffect(),

            new EmbossEffect(),
            new StraightenEffect(),
        };

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
                    con.Frame.Child = null;
                    con.FrameVisibility = false;
                }

                foreach (var effect in con.Effects)
                {
                    EffectItem effectItem = effect.GetItem(layer.EffectManager);
                    effect.IsOn = effectItem.IsOn;
                }
                con.IsEnabled = true;

            }
            else
            {
                con.IsEnabled = false;

                con.Frame.Child = null;
                con.FrameVisibility = false;
            }
        }));

        #endregion
   

        public EffectsControl()
        {
            this.InitializeComponent();

            this.IsEnabled =
            this.FrameVisibility = false;
            this.ItemsControl.ItemsSource = this.Effects;

            //Button
            this.BackButton.Tapped += (sender, e) => this.Clear();
            this.ResetButton.Tapped += (sender, e) => this.Reset();
        }


        //Effect
        private void EffectControl_EffectToggle(Effect effect) => this .Toggle(effect);
        private void EffectControl_EffectTapped(Effect effect) => this.Effect = effect;


        /// <summary> Toggle Effect's IsOn. </summary>
        private void Toggle(Effect effect)
        {
            if (this.Layer == null) return;


            bool value = effect.ToggleSwitchIsOn;

            effect.IsOn = value;

            EffectItem effectItem = effect.GetItem(this.Layer.EffectManager);
            effectItem.IsOn = value;


            this.ViewModel.Invalidate();
        }
        

        /// <summary> Reset the Effect. </summary>
        private void Reset()
        {
            if (this.Effect == null) return;

            this.Effect.Reset(this.Layer.EffectManager);
            this.Effect.SetPage(this.Layer.EffectManager);

            this.ViewModel.Invalidate();
        }
        /// <summary> Clear the Effect. </summary>
        private void Clear()
        {
            this.Effect = null;
            this.Frame.Child = null;
            this.FrameVisibility = false;
        } 
    }
}
