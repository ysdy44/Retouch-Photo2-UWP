using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "EffectMenu" />. 
    /// </summary>
    public sealed partial class EffectMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        

        private IEffect effect;
        public IEffect Effect
        {
            get => this.effect;
            set
            {
                if (this.effect == value) return;

                if (value == null)
                    this.EffectBoder.Child = null;
                else
                    this.EffectBoder.Child = value.Page;

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

        /// <summary> Gets or sets <see cref = "EffectMenu" />'s EffectManager. </summary>
        public EffectManager EffectManager
        {
            get { return (EffectManager)GetValue(EffectManagerProperty); }
            set { SetValue(EffectManagerProperty, value); }
        }
        /// <summary> Identifies the <see cref = "EffectMenu.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty EffectManagerProperty = DependencyProperty.Register(nameof(EffectManager), typeof(EffectManager), typeof(EffectMenu), new PropertyMetadata(null, (sender, e) =>
        {
            EffectMenu con = (EffectMenu)sender;

            if (e.NewValue is EffectManager value)
            {
                foreach (IEffect effect in con.Effects)
                {
                    effect.ToggleSwitch.IsEnabled = true;

                    //IsOn
                    effect.FollowEffectManager(value);
                }

                con._Expander.IsSecondPage = false;
            }
            else
            {
                foreach (IEffect effect in con.Effects)
                {
                    effect.ToggleSwitch.IsEnabled = false;
                }
 
                con._Expander.IsSecondPage = false;
            }
        }));

        #endregion


        //@Construct
        public EffectMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.ConstructEffects();                        
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "EffectMenu" />. 
    /// </summary>
    public sealed partial class EffectMenu : UserControl, IMenu
    {
        //Effects
        private void ConstructEffects()
        {
            foreach (IEffect effect in this.Effects)
            {
                this.EffectsStackPanel.Children.Add(effect.Button);

                effect.ToggleSwitch.Toggled += (s, e) => this.Overwriting(effect);

                effect.Button.Tapped += (s, e) => this.Navigate(effect);
            }
        }

        //Reset
        private void Reset()
        {
            if (this.Effect == null) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                EffectManager effectManager = layer.EffectManager;

                this.Effect.Reset();
                this.Effect.ResetEffectManager(effectManager);
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //Overwriting
        public void Overwriting(IEffect effect)
        {
            if (effect == null) return;
            if (effect.ToggleSwitch.IsEnabled == false) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                EffectManager effectManager = layer.EffectManager;
                effect.OverwritingEffectManager(effectManager);
            });
            this.ViewModel.Invalidate();//Invalidate
        }

        //Navigate
        public void Navigate(IEffect effect)
        {
            if (effect == null) return;
            if (effect.ToggleSwitch.IsEnabled == false) return;
            if (effect.ToggleSwitch.IsOn == false) return;

            this.Effect = effect;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                EffectManager effectManager = layer.EffectManager;
                effect.FollowEffectManager(effectManager);

                return;
            });

            this._Expander.IsSecondPage = true;
        }
    }

    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "EffectMenu" />. 
    /// </summary>
    public sealed partial class EffectMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Menus/Effect");
            this._Expander.Title = resource.GetString("/Menus/Effect");
        }
        
        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }


        //@Content
        public MenuType Type => MenuType.Effect;
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public Point Postion { get; set; }
        public FrameworkElement Layout => this;
        public FrameworkElement Button => this._button;
        private MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Effects.Icon()
        };

        public MenuState State
        {
            get => this.state;
            set
            {
                this._button.State = value;
                this._Expander.State = value;
                MenuHelper.SetMenuState(value, this);
                this.state = value;
            }
        }
        private MenuState state;


        //@Construct  
        public void ConstructMenu()
        {
            this.State = MenuState.Hide;
            this.Button.Tapped += (s, e) => this.State = MenuHelper.GetState(this.State);
            this._Expander.CloseButton.Tapped += (s, e) => this.State = MenuState.Hide;
            this._Expander.StateButton.Tapped += (s, e) => this.State = MenuHelper.GetState2(this.State);
            this._Expander.ResetButton.Tapped += (s, e) => this.Reset(); 
            this._Expander.BackButton.Tapped += (s, e) => this._Expander.IsSecondPage = false;
            MenuHelper.ConstructTitleGrid(this._Expander.TitleGrid, this);
        }


    }
}
