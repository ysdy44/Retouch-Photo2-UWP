using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

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
        /// <summary> Identifies the <see cref = "EffectMenu.EffectManager" /> dependency property. </summary>
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
            this.ConstructDataContext
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.EffectManager),
                 dp: EffectMenu.EffectManagerProperty
            );
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
        //DataContext
        public void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
        {
            this.DataContext = dataContext;

            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Menus/Effect");
            this._Expander.Title = resource.GetString("/Menus/Effect");
        }

        //Menu
        public MenuType Type => MenuType.Effect;
        public IExpander Expander => this._Expander;
        MenuButton _button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Effects.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Reset = this.Reset;
            this._Expander.Initialize();
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
            this._Expander.ResetButtonVisibility = Visibility.Visible;
        }
    }
}