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
    /// Menu of <see cref = "Retouch_Photo2.Styles.IStyle"/>.
    /// </summary>
    public sealed partial class StyleMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.StyleMainPage;

        readonly StyleMainPage StyleMainPage = new StyleMainPage();


        //@Construct
        /// <summary>
        /// Initializes a StyleMenu. 
        /// </summary>
        public StyleMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }
            
    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Styles.IStyle"/>.
    /// </summary>
    public sealed partial class StyleMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("/Menus/Style");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Style;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Styles.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }   
}