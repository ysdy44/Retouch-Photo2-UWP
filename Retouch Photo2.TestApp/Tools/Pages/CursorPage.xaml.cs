using Retouch_Photo2.TestApp.Tools.Models;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Tools.Pages
{
    /// <summary>
    /// <see cref="CursorTool"/>'s Page.
    /// </summary>
    public sealed partial class CursorPage : Page
    {
        //ViewModel
        public ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;
        SelectionViewModel Selection => Retouch_Photo2.TestApp.App.Selection;
        KeyboardViewModel Keyboard => Retouch_Photo2.TestApp.App.Keyboard;
        
        //@Construct
        public CursorPage()
        {
            this.InitializeComponent();
        }
    }
}