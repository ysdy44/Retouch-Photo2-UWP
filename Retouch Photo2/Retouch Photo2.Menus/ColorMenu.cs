// Core:              ★★
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "HSVColorPickers.ColorPicker"/>.
    /// </summary>
    public sealed partial class ColorMenu : Expander, IMenu 
    {

        //@ViewModel
        ViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content     
        public override UIElement MainPage => this.ColorMainPage;

        readonly ColorMainPage ColorMainPage = new ColorMainPage();


        //@Construct
        /// <summary>
        /// Initializes a ColorMenu. 
        /// </summary>
        public ColorMenu()
        {
            this.InitializeComponent();
            this.Button.CenterContent = new ColorEllipse
            (
                 dataContext: this.SelectionViewModel,
                 path: nameof(this.SelectionViewModel.Color),
                 dp: ColorEllipse.ColorProperty
            );
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "HSVColorPickers.ColorPicker"/>.
    /// </summary>
    public sealed partial class ColorMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            base.Title = resource.GetString("/Menus/Color");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Transformer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton();
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }    
}