using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Effects;
using Retouch_Photo.Effects.Models;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Retouch_Photo.Controls
{
    public sealed partial class EffectsControl : UserControl
    {

        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
                
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
            new Retouch_Photo.Effects.Models.GaussianBlurEffect(),
            new Retouch_Photo.Effects.Models.DirectionalBlurEffect(),
            new Retouch_Photo.Effects.Models.OuterShadowEffect(),

            new Retouch_Photo.Effects.Models.OutlineEffect(),

            new Retouch_Photo.Effects.Models.EmbossEffect(),
            new Retouch_Photo.Effects.Models.StraightenEffect(),
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
