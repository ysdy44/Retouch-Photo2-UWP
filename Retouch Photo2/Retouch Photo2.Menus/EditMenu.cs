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
    /// Menu of <see cref = "Retouch_Photo2.Edits"/>.
    /// </summary>
    public sealed partial class EditMenu : Expander, IMenu 
    {


        //@Content       
        public override UIElement MainPage => this.EditMainPage;

        readonly EditMainPage EditMainPage = new EditMainPage();


        //@Construct
        /// <summary>
        /// Initializes a EditMenu. 
        /// </summary>
        public EditMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Edits"/>.
    /// </summary>
    public sealed partial class EditMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("/Menus/Edit");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Edit;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Edits.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }   
}