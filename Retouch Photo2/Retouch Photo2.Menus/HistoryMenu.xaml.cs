// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★★
// Only:              
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMenu : Expander, IMenu 
    {

        //@Content     
        public override UIElement MainPage => this.HistoryMainPage;
        HistoryMainPage HistoryMainPage = new HistoryMainPage();


        //@Construct
        /// <summary>
        /// Initializes a HistoryMenu. 
        /// </summary>
        public HistoryMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title =
            this.Title = resource.GetString("/Menus/History");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.History;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Historys.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }

    
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Historys.IHistory"/>.
    /// </summary>
    public sealed partial class HistoryMainPage : UserControl
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;


        //@Construct
        /// <summary>
        /// Initializes a HistoryMainPage. 
        /// </summary>
        public HistoryMainPage()
        {
            this.InitializeComponent();

            this.ListView.ItemsSource = this.ViewModel.Historys;
        }
    }
}