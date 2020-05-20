using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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

        
        private IEffectPage currentEffect;
        public IEffectPage CurrentEffect
        {
            get => this.currentEffect;
            set
            {
                if (this.currentEffect == value) return;

                if (value == null)
                    this.EffectBoder.Child = null;
                else
                    this.EffectBoder.Child = value.Page;

                this.currentEffect = value;
            }
        }
        public List<IEffectPage> Effects = new List<IEffectPage>
        {
            new GaussianBlurEffectPage(),
            new DirectionalBlurEffectPage(),
            new SharpenEffectPage(),
            new OuterShadowEffectPage(),

            new OutlineEffectPage(),

            new EmbossEffectPage(),
            new StraightenEffectPage(),
        };

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "EffectMenu" />'s Effect. </summary>
        public Effect Effect
        {
            get { return (Effect)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }
        /// <summary> Identifies the <see cref = "EffectMenu.Effect" /> dependency property. </summary>
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(nameof(Effect), typeof(Effect), typeof(EffectMenu), new PropertyMetadata(null, (sender, e) =>
        {
            EffectMenu con = (EffectMenu)sender;

            if (e.NewValue is Effect value)
            {
                foreach (IEffectPage effect in con.Effects)
                {
                    effect.ToggleSwitch.IsEnabled = true;

                    //IsOn
                    effect.FollowEffect(value);
                }

                con._Expander.IsSecondPage = false;
            }
            else
            {
                foreach (IEffectPage effect in con.Effects)
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
                 path: nameof(this.SelectionViewModel.Effect),
                 dp: EffectMenu.EffectProperty
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

            this._button.ToolTip.Content = 
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Effect");
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
            foreach (IEffectPage effect in this.Effects)
            {
                this.EffectsStackPanel.Children.Add(effect.Button);

                effect.ToggleSwitch.Toggled += (s, e) => this.Overwriting(effect);

                effect.Button.Tapped += (s, e) => this.Navigate(effect);
            }
        }

        //Reset
        private void Reset()
        {
            if (this.CurrentEffect == null) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                Effect effect = layer.Effect;

                this.CurrentEffect.Reset();
                this.CurrentEffect.ResetEffect(effect);
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //Overwriting
        public void Overwriting(IEffectPage effect)
        {
            if (effect == null) return;
            if (effect.ToggleSwitch.IsEnabled == false) return;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                Effect effect2 = layer.Effect;
                effect.OverwritingEffect(effect2);
            });
            this.ViewModel.Invalidate();//Invalidate
        }

        //Navigate
        public void Navigate(IEffectPage effect)
        {
            if (effect == null) return;
            if (effect.ToggleSwitch.IsEnabled == false) return;
            if (effect.ToggleSwitch.IsOn == false) return;

            this.CurrentEffect = effect;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                Effect effect2 = layer.Effect;
                effect.FollowEffect(effect2);

                return;
            });

            this._Expander.IsSecondPage = true;
            this._Expander.ResetButtonVisibility = Visibility.Visible;
            this._Expander.CurrentTitle = effect.Button.Text;
        }
    }
}