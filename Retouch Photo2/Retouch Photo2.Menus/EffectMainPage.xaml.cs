// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Effects;
using Retouch_Photo2.Effects.Models;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
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
            new DirectionalBlurEffectPage(),
            new SharpenEffectPage(),
            new OuterShadowEffectPage(),

            new EdgeEffectPage(),
            new MorphologyEffectPage(),

            new EmbossEffectPage(),
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
                    effectPage.Button.IsEnabled = true;
                    effectPage.FollowButton(value);

                    if (effectPage == control.EffectPage)
                    {
                        effectPage.FollowPage(value);
                    }
                }
            }
            else
            {
                foreach (IEffectPage effect in control.EffectPages)
                {
                    effect.Button.IsEnabled = false;
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
                this.EffectsStackPanel.Children.Add(effectPage.Button);

                effectPage.Button.Tapped += (s, e) => this.Navigate(effectPage);
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
            IEnumerable<Layerage> selectedLayerages = LayerageCollection.GetAllSelected(this.ViewModel.LayerageCollection);
            Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayerages);
            ILayer layer = outermost.Self;

            Effect effect = layer.Effect;
            effectPage.FollowPage(effect);
            this.EffectPage = effectPage;

            string title = effectPage.Button.Text;
            UIElement secondPage = effectPage.Page;
            this.SecondPageChanged?.Invoke(title, secondPage);//Delegate
        }
    }
}