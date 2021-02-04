// Core:              ★★
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.OperateMainPage;

        readonly OperateMainPage OperateMainPage = new OperateMainPage();


        //@Construct
        /// <summary>
        /// Initializes a OperateMenu. 
        /// </summary>
        public OperateMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Operates"/>.
    /// </summary>
    public sealed partial class OperateMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Operate");

            this.Button.ToolTip.Closed += (s, e) => this.OperateMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.OperateMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Operate;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Operates.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}