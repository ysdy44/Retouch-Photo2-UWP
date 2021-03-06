// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Effects.Effect"/>.
    /// </summary>
    public sealed partial class EffectMenu : Expander, IMenu
    {

        //@Content     
        public bool IsOpen { set { } }
        public override UIElement MainPage => this.EffectMainPage;

        readonly EffectMainPage EffectMainPage = new EffectMainPage();


        //@Construct
        /// <summary>
        /// Initializes a EffectMenu. 
        /// </summary>
        public EffectMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.EffectMainPage.IsSecondPageChanged += (s, isSecondPage) => this.Back();
            this.EffectMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
                this.ResetButtonVisibility = Visibility.Visible;
            };
        }
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Effects.Effect"/>.
    /// </summary>
    public sealed partial class EffectMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("Menus_Effect");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Effect;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            Content = new Retouch_Photo2.Effects.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset()
        {
            this.EffectMainPage.Reset();
        }

    }

    /// <summary>
    /// MainPage of <see cref = "EffectMenu"/>.
    /// </summary>
    public sealed partial class EffectMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Delegate
        /// <summary> Occurs when is-second-page change. </summary>
        public event EventHandler<bool> IsSecondPageChanged;
        /// <summary> Occurs when second-page change. </summary>
        public event EventHandler<UIElement> SecondPageChanged;


        IEffectPage EffectPage = null;

        /// <summary> Gets the effect pages. </summary>
        public IList<IEffectPage> EffectPages { get; } = new List<IEffectPage>
        {
            new GaussianBlurEffectPage(),
            null,
            new DirectionalBlurEffectPage(),
            null,
            new SharpenEffectPage(),
            null,
            new OuterShadowEffectPage(),
            null,

            new EdgeEffectPage(),
            null,
            new MorphologyEffectPage(),
            null,

            new EmbossEffectPage(),
            null,
            new StraightenEffectPage(),
        };

        #region DependencyProperty

        /// <summary> Gets or sets <see cref = "EffectMainPage" />'s Effect. </summary>
        public Effect Effect
        {
            get => (Effect)base.GetValue(EffectProperty);
            set => base.SetValue(EffectProperty, value);
        }
        /// <summary> Identifies the <see cref = "EffectMainPage.Effect" /> dependency property. </summary>
        public static readonly DependencyProperty EffectProperty = DependencyProperty.Register(nameof(Effect), typeof(Effect), typeof(EffectMainPage), new PropertyMetadata(null, (sender, e) =>
        {
            EffectMainPage control = (EffectMainPage)sender;

            if (e.NewValue is Effect value)
            {
                foreach (IEffectPage effectPage in control.EffectPages)
                {
                    if (effectPage == null) continue;

                    effectPage.ToggleButton.IsEnabled = true;
                    effectPage.Button.IsEnabled = effectPage.ToggleButton.IsChecked;
                    effectPage.FollowButton(value);

                    if (effectPage == control.EffectPage)
                    {
                        effectPage.FollowPage(value);
                    }
                }
            }
            else
            {
                foreach (IEffectPage effectPage in control.EffectPages)
                {
                    if (effectPage == null) continue;

                    effectPage.Button.IsEnabled = false;
                    effectPage.ToggleButton.IsEnabled = false;
                }
            }

            control.IsSecondPageChanged?.Invoke(control, false);//Delegate
        }));

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a EffectMainPage. 
        /// </summary>
        public EffectMainPage()
        {
            this.InitializeComponent();
            this.ConstructDataContext
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.Effect),
                 dp: EffectMainPage.EffectProperty
            );

            this.ConstructEffects();
        }
    }

    /// <summary>
    /// MainPage of <see cref = "EffectMenu"/>.
    /// </summary>
    public sealed partial class EffectMainPage : UserControl
    {

        //DataContext
        private void ConstructDataContext(object dataContext, string path, DependencyProperty dp)
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

    }

    /// <summary>
    /// MainPage of <see cref = "EffectMenu"/>.
    /// </summary>
    public sealed partial class EffectMainPage : UserControl
    {

        //Effects
        private void ConstructEffects()
        {
            foreach (IEffectPage effectPage in this.EffectPages)
            {
                if (effectPage == null)
                {
                    this.ButtonsStackPanel.Children.Add(new Rectangle
                    {
                        Style = this.SeparatorRectangle
                    });
                    this.ToggleButtonsStackPanel.Children.Add(new Rectangle
                    {
                        Style = this.SeparatorRectangle2
                    });
                }
                else
                {
                    this.ButtonsStackPanel.Children.Add(effectPage.Button);
                    this.ToggleButtonsStackPanel.Children.Add(effectPage.ToggleButton);

                    effectPage.Button.Tapped += (s, e) => this.Navigate(effectPage);
                }
            }
        }

        //Reset
        public void Reset()
        {
            if (this.EffectPage is IEffectPage effectPage)
            {
                effectPage.Reset();
                this.ViewModel.Invalidate();//Invalidate
            }
        }

        //Navigate
        public void Navigate(IEffectPage effectPage)
        {
            if (effectPage == null) return;

            //Layers
            IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();
            Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayerages);
            ILayer layer = outermost.Self;

            Effect effect = layer.Effect;
            effectPage.FollowPage(effect);
            this.EffectPage = effectPage;

            string title = (string)effectPage.Button.Content;
            UIElement secondPage = effectPage.Page;
            this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
        }
    }
}