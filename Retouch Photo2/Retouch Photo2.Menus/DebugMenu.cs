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
    /// Menu of Debug.
    /// </summary>
    public sealed partial class DebugMenu : Expander, IMenu
    {       
        //@Content     
        public override UIElement MainPage => this.DebugMainPage;

        readonly DebugMainPage DebugMainPage = new DebugMainPage();

        //@Construct
        /// <summary>
        /// Initializes a DebugMenu. 
        /// </summary>
        public DebugMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }
    }

    /// <summary>
    /// Menu of Debug.
    /// </summary>
    public sealed partial class DebugMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = "Debug";
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Debug;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = "?"
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}