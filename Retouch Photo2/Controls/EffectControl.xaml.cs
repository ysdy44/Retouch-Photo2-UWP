using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary>
    /// Retouch_Photo2's the only <see cref = "EffectControl" />. 
    /// </summary>
    public sealed partial class EffectControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => Retouch_Photo2.App.KeyboardViewModel;


        /// <summary> State of <see cref="EffectControl"/>. </summary>
        public EffectsState State
        {
            get => this.state;
            set
            {
                this.ItemsControl.Visibility = (value == EffectsState.Edit) ? Visibility.Collapsed : Visibility.Visible;

                this.BackButton.Visibility =
                this.ResetButton.Visibility =
                this.Frame.Visibility =
                    (value == EffectsState.Edit) ? Visibility.Visible : Visibility.Collapsed;

                this.state = value;
            }
        }
        private EffectsState state;


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

                con.State = EffectsState.Effects;//State
            }
            else
            {
                foreach (var effect in con.Effects)
                {
                    effect.Button.IsOn = null;
                }

                con.State = EffectsState.Disable;//State
            }
        }));

        #endregion


        //@Construct
        public EffectControl()
        {
            this.InitializeComponent();
            this.State = EffectsState.Disable;
            this.ItemsControl.ItemsSource = from item in this.Effects select item.Button;

            foreach (Effect effect in this.Effects)
            {
                //Binding
                this.Binding(effect);
            }
            
            //Effect
            Retouch_Photo2.Effects.EffectManager.InvalidateAction = (Action<EffectManager> action) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    action(layer.EffectManager);
                });

                this.ViewModel.Invalidate();//Invalidate
            };


            //Button
            this.BackButton.Tapped += (s, e) =>
            {
                this.Effect = null;
                this.Frame.Child = null;
                this.State = EffectsState.Effects;
            };
            this.ResetButton.Tapped += (s, e) =>
            {
                if (this.Effect == null) return;
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    EffectManager effectManager = layer.EffectManager;
                    this.Effect.Reset(effectManager);
                    this.Effect.SetPageValueByEffectManager(effectManager);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        /// <summary>
        /// Bind the events of the buttons in each effect.
        /// </summary>
        /// <param name="effect"> Effect </param>
        private void Binding(Effect effect)
        {
            //ToggleSwitch
            effect.Button.ToggleSwitch.Toggled += (s, e) =>
            {
                bool isOn = effect.Button.ToggleSwitch.IsOn;
                effect.Button.IsOn = isOn; 
                
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    effect.SetIsOn(layer.EffectManager, isOn);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            //RootButton
            effect.Button.RootButton.Tapped += (s, e) =>
            {
                this.Effect = effect;
                this.Frame.Child = effect.Page;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    effect.SetPageValueByEffectManager(layer.EffectManager);
                });

                this.State = EffectsState.Edit;
            };
        }

    }
}