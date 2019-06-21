using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// <see cref="CursorTool"/>'s Page.
    /// </summary>
    public sealed partial class CursorPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => Retouch_Photo2.App.KeyboardViewModel;
        TipViewModel TipViewModel => Retouch_Photo2.App.TipViewModel;

        //@Construct
        public CursorPage()
        {
            this.InitializeComponent();
        }
    }
}