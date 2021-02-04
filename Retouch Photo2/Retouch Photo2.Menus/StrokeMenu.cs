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
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.StrokeMainPage;

        readonly StrokeMainPage StrokeMainPage = new StrokeMainPage();


        //@Construct
        /// <summary>
        /// Initializes a StrokeMenu. 
        /// </summary>
        public StrokeMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();            
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Stroke");

            this.Button.ToolTip.Closed += (s, e) => this.StrokeMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.StrokeMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Stroke;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Strokes.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}