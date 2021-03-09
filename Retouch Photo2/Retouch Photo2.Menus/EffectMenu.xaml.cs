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
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
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
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Content     
        public bool IsOpen { set { } }
        public override UIElement MainPage => this.EffectMainPage;

        readonly EffectMainPage EffectMainPage = new EffectMainPage();
       
        IEffectPage EffectPage = null;


        //@Construct
        /// <summary>
        /// Initializes a EffectMenu. 
        /// </summary>
        public EffectMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            foreach (IEffectPage  effectPage in this.EffectMainPage.EffectPages)
            {
                if (effectPage == null) continue;

                effectPage.Button.Click += (s, e) => this.Navigate(effectPage);
            }
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
            if (this.EffectPage is IEffectPage effectPage)
            {
                effectPage.Reset();
                this.ViewModel.Invalidate();//Invalidate
            }
        }

        //Navigate
        public void Navigate(IEffectPage effectPage)
        {
            //Layers
            IEnumerable<Layerage> selectedLayerages = LayerManager.GetAllSelected();
            Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayerages);
            if (outermost == null) return;
            ILayer layer = outermost.Self;

            Effect effect = layer.Effect;
            effectPage.FollowPage(effect);
            this.EffectPage = effectPage;

            string title = (string)effectPage.Button.Content;
            UIElement secondPage = effectPage.Page;
            this.SecondPageChanged(title, secondPage);//Delegate
        }

        private void SecondPageChanged(string title, UIElement secondPage)
        {
            if (this.Page != secondPage) this.Page = secondPage;
            this.IsSecondPage = true;
            this.Title = (string)title;
            this.ResetButtonVisibility = Visibility.Visible;
        }

    }

    /// <summary>
    /// MainPage of <see cref = "EffectMenu"/>.
    /// </summary>
    public sealed partial class EffectMainPage : UserControl
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;

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

        //@Construct
        /// <summary>
        /// Initializes a EffectMainPage. 
        /// </summary>
        public EffectMainPage()
        {
            this.InitializeComponent();
            this.EffectControl.EffectPages = this.EffectPages;
            this.EffectControl.ConstructString(this.EffectPages);
            this.EffectControl.ConstructButton(this.EffectPages);
            this.EffectControl.ConstructToggleButton(this.EffectPages);
        }



    }
}