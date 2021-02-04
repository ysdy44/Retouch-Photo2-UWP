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
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : Expander, IMenu
    {

        //@Content     
        public override UIElement MainPage => this.TextMainPage;

        readonly TextMainPage TextMainPage = new TextMainPage();


        //@Construct
        /// <summary>
        /// Initializes a TextMenu. 
        /// </summary>
        public TextMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            
            this.TextMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Texts"/>.
    /// </summary>
    public sealed partial class TextMenu : Expander, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Text");

            this.Button.ToolTip.Closed += (s, e) => this.TextMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.TextMainPage.IsOpen = true;
            };
        }

        //Menu  
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Text;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Texts.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}