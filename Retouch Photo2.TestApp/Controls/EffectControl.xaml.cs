using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> State of <see cref="EffectControl"/>. </summary>
    public enum EffectsControlState
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Disable. </summary>
        Disable,

        /// <summary> Effects display Mode. </summary>
        Effects,

        /// <summary> Effects edited Mode. </summary>
        Edit
    }

    /// <summary>
    /// Retouch_Photo2's the only <see cref = "EffectControl" />. 
    /// </summary>
    public sealed partial class EffectControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        /// <summary> State of <see cref="EffectControl"/>. </summary>
        public EffectsControlState State
        {
            get => this.state;
            set
            {
                this.ItemsControl.Visibility = (value == EffectsControlState.Edit) ? Visibility.Collapsed : Visibility.Visible;

                this.BackButton.Visibility =
                this.ResetButton.Visibility =
                this.Frame.Visibility =
                    (value == EffectsControlState.Edit) ? Visibility.Visible : Visibility.Collapsed;

                this.state = value;
            }
        }
        private EffectsControlState state;


        Effect Effect;
        List<Effect> Effects = new List<Effect>
        {
            new GaussianBlurEffect(),
            new DirectionalBlurEffect(),
            new SharpenEffect(),
            new OuterShadowEffect(),

            new OutlineEffect(),

            new EmbossEffect(),
            new StraightenEffect(),
        };


        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "EffectControl" />'s EffectManager. </summary>
        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "EffectControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectControl), new PropertyMetadata(null, (sender, e) =>
        {
            EffectControl con = (EffectControl)sender;

            if (e.NewValue is EffectManager value)
            {
                foreach (var effect in con.Effects)
                {
                    bool isOn = effect.GetIsOn(value);
                    effect.Button.IsOn = isOn;
                }

                con.State = EffectsControlState.Effects;//State
            }
            else
            {
                foreach (var effect in con.Effects)
                {
                    effect.Button.IsOn = null;
                }

                con.State = EffectsControlState.Disable;//State
            }
        }));

        #endregion


        //@Construct
        public EffectControl()
        {
            this.InitializeComponent();
            this.State = EffectsControlState.Disable;
            this.ItemsControl.ItemsSource = from item in this.Effects select item.Button;

            foreach (Effect  effect in this.Effects)
            {
                //Binding
                this.Binding(effect);
            }
            
            //Effect
            Retouch_Photo2.Effects.EffectManager.InvalidateAction = (Action<EffectManager> action) =>
            {
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    action(layer.EffectManager);
                });
                this.ViewModel.Invalidate();
            };


            //Button
            this.BackButton.Tapped += (s, e) =>
            {
                this.Effect = null;
                this.Frame.Child = null;
                this.State = EffectsControlState.Effects;
            };
            this.ResetButton.Tapped += (s, e) =>
            {
                if (this.Effect == null) return;

                this.Effect.Reset(this.ViewModel.SelectionIsEffectManager);

                this.ViewModel.Invalidate();
            };
        }


        private void Binding(Effect effect)
        {
            //ToggleSwitch
            effect.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                bool isOn = effect.Button.ToggleSwitch.IsOn;
                if (isOn == effect.Button.IsOn) return;

                effect.Button.IsOn = isOn;
                effect.SetIsOn(this.EffectManager, isOn);

                this.ViewModel.Invalidate();
            };

            //RootButton
            effect.Button.RootButton.Tapped += (s, e) =>
            {
                this.Effect = effect;
                this.Frame.Child = effect.Page;

                effect.SetPageValueByEffectManager(this.EffectManager);

                this.State = EffectsControlState.Edit;
            };
        }
    }
}