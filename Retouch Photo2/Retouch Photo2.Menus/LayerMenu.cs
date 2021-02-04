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
    /// Menu of <see cref = "Retouch_Photo2.Layers.ILayer"/>.
    /// </summary>
    public sealed partial class LayerMenu : Expander, IMenu
    {

        //@Content     
        public override UIElement MainPage => this.LayerMainPage;

        readonly LayerMainPage LayerMainPage = new LayerMainPage();


        //@Construct
        /// <summary>
        /// Initializes a LayerMenu. 
        /// </summary>
        public LayerMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.LayerMainPage.SecondPageChanged += (title, secondPage) =>
            {
                if (this.Page != secondPage) this.Page = secondPage;
                this.IsSecondPage = true;
                this.Title = (string)title;
            };
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Layers.ILayer"/>.
    /// </summary>
    public sealed partial class LayerMenu : Expander, IMenu
    {

        //DataContext
        private void ConstructDataContext(string path, DependencyProperty dp)
        {
            // Create the binding description.
            Binding binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(path)
            };

            // Attach the binding to the target.
            this.SetBinding(dp, binding);
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Layer");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Layer;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Layers.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }
}