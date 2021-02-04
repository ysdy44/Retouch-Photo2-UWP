// Core:              ★★
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Retouch_Photo2.Elements;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.TransformerMainPage;

        readonly TransformerMainPage TransformerMainPage = new TransformerMainPage();


        //@Construct
        /// <summary>
        /// Initializes a TransformerMenu. 
        /// </summary>
        public TransformerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.TransformerMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "FanKit.Transformers.Transformer"/>.
    /// </summary>
    public sealed partial class TransformerMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Transformer");

            this.Button.ToolTip.Closed += (s, e) => this.TransformerMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.TransformerMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Transformer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new FanKit.Transformers.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}