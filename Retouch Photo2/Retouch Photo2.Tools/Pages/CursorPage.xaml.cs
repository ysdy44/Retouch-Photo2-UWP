using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "CursorTool"/>.
    /// </summary>
    public sealed partial class CursorPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }

        //@Construct
        public CursorPage()
        {
            this.InitializeComponent();
        }
    }
}