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
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.AdjustmentMainPage;

        readonly AdjustmentMainPage AdjustmentMainPage = new AdjustmentMainPage();
        

        //@Construct
        /// <summary>
        /// Initializes a AdjustmentMenu. 
        /// </summary>
        public AdjustmentMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.AdjustmentMainPage.IsSecondPageChanged += (s, isSecondPage) => this.Back();
            this.AdjustmentMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
                this.ResetButtonVisibility = Visibility.Visible;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Adjustments.IAdjustment"/>.
    /// </summary>
    public sealed partial class AdjustmentMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("/Menus/Adjustment");
        }

        //Menu      
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Adjustment;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Adjustments.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset()
        {
            this.AdjustmentMainPage.Reset();
        }

    }
}