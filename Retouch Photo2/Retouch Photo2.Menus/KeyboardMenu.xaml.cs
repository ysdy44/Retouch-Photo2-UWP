using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Elements;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of Keyboard.
    /// </summary>
    public sealed partial class KeyboardMenu : Expander, IMenu 
    {
        //@Content
        TextMainPage TextMainPage = new TextMainPage();

        //@Construct
        /// <summary>
        /// Initializes a KeyboardMenu. 
        /// </summary>
        public KeyboardMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.MainPage = this.TextMainPage;
        }
    }

    /// <summary>
    /// Menu of Keyboard.
    /// </summary>
    public sealed partial class KeyboardMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = "Keyboard";
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Keyboard;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = "Key"
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }



    /// <summary>
    /// MainPage of <see cref="KeyboardMenu"/>.
    /// </summary>
    public sealed partial class KeyboardMainPage : UserControl
    {
        //@Construct
        /// <summary>
        /// Initializes a KeyboardMainPage. 
        /// </summary>
        public KeyboardMainPage()
        {
            this.InitializeComponent();
        }
    }
}