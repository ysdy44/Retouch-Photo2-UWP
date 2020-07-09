using System;
using System.Collections.Generic;
using System.IO;
using Retouch_Photo2.Elements;
using Windows.ApplicationModel.Resources;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of Debug.
    /// </summary>
    public sealed partial class DebugMenu : Expander, IMenu
    {       
        //@Content     
        public override UIElement MainPage => this.TextMainPage;
        TextMainPage TextMainPage = new TextMainPage();

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


    /// <summary>
    /// MainPage of <see cref = "DebugMenu"/>.
    /// </summary>
    public sealed partial class DebugMainPage : Page
    {
        //@Construct
        /// <summary>
        /// Initializes a DebugMainPage. 
        /// </summary>
        public DebugMainPage()
        {
            this.InitializeComponent();
        }
    }
}
