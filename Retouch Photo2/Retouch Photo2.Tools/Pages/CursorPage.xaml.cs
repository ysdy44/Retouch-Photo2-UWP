using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "CursorTool"/>.
    /// </summary>
    public sealed partial class CursorPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;

        //@Construct
        public CursorPage()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}