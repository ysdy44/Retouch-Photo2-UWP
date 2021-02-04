// Core:              ★★
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Retouch_Photo2.Elements;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Effects.Effect"/>.
    /// </summary>
    public sealed partial class EffectMenu : Expander, IMenu 
    {

        //@Content     
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
            this.Title = resource.GetString("/Menus/Effect");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Effect;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Effects.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset()
        {
            this.EffectMainPage.Reset();
        }

    }   
}