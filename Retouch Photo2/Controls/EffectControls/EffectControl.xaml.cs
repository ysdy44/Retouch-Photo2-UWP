using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using System;
using System.Collections.Generic;
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
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;


        //@Content
        public MenuTitle MenuTitle => this._MenuTitle;


        /// <summary> Manager of <see cref="EffectControlState"/>. </summary>
        EffectControlStateManager Manager = new EffectControlStateManager();
        /// <summary> State of <see cref="EffectControl"/>. </summary>
        EffectControlState State
        {
              set
             {
                 if (value == EffectControlState.Edit)
                 {
                     this.MenuTitle.IsSecondPage = true;
                     this.StackPanel.Visibility = Visibility.Collapsed;
                     this.EffectBoder.Visibility = Visibility.Visible;
                 }
                 else
                 {
                     this.MenuTitle.IsSecondPage = false;
                     this.StackPanel.Visibility = Visibility.Visible;
                     this.EffectBoder.Visibility = Visibility.Collapsed;
                 }
             }
        }


        private IEffect effect;
        public IEffect Effect
        {
            get => this.effect;
            set
            {
                if (value == null)
                    this.EffectBoder.Child = null;
                else
                    this.EffectBoder.Child = value.Page.Self;

                this.effect = value;
            }
        }
        public List<IEffect> Effects = new List<IEffect>
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
                foreach (IEffect effect in con.Effects)
                {
                    //IsEnabled
                    effect.Button.ToggleSwitch.IsEnabled = true;

                    //IsOn
                    effect.Button.FollowEffectManager(value);
                }

                con.Manager.IsEdit = false;
                con.Manager.ExistEffect = true;
                con.State = con.Manager.GetState();//State
            }
            else
            {
                foreach (IEffect effect in con.Effects)
                {
                    //IsEnabled
                    effect.Button.ToggleSwitch.IsEnabled = false;
                }

                con.Manager.IsEdit = false;
                con.Manager.ExistEffect = true;
                con.State = con.Manager.GetState();//State
            }
        }));

        #endregion


        //@Construct
        public EffectControl()
        {
            this.InitializeComponent();
            this.State = EffectControlState.Disable;

            foreach (IEffect effect in this.Effects)
            {
                UIElement button = effect.Button.Self;
                this.StackPanel.Children.Add(button);

                //Switch
                effect.Button.ToggleSwitch.Toggled += (s, e) => this.Overwriting(effect);

                //Navigate
                effect.Button.Self.Tapped += (s, e) => this.Navigate(effect);
            }

            //Effect
            EffectManager.InvalidateAction = (Action<EffectManager> action) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    action(layer.EffectManager);
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            //Button
            this.MenuTitle.ResetButton.Tapped += (s, e) =>
            {
                if (this.Effect == null) return;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    IEffectPage page = this.Effect.Page;
                    page.Reset();

                    EffectManager effectManager = layer.EffectManager;
                    page.ResetEffectManager(effectManager);
                });

                this.ViewModel.Invalidate();//Invalidate
                return;
            };

            this.MenuTitle.BackButton.Tapped += (s, e) =>
            {
                this.Effect = null;

                this.Manager.IsEdit = false;
                this.State = this.Manager.GetState();//State
            };
        }



     

        public void Overwriting(IEffect effect)
        {
            if (effect == null) return;
            if (effect.Button.ToggleSwitch.IsEnabled == false) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                EffectManager effectManager = layer.EffectManager;
                effect.Button.OverwritingEffectManager(effectManager);
            });
            this.ViewModel.Invalidate();//Invalidate
        }

        public void Navigate(IEffect effect)
        {
            if (effect == null) return;
            if (effect.Button.ToggleSwitch.IsEnabled == false) return;
            if (effect.Button.ToggleSwitch.IsOn == false) return;
            
            this.Effect = effect;
            IEffectPage page = effect.Page;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                EffectManager effectManager = layer.EffectManager;
                page.FollowEffectManager(effectManager);

                return;
            });

            this.Manager.IsEdit = true;
            this.State = this.Manager.GetState();//State
        }

    }
}