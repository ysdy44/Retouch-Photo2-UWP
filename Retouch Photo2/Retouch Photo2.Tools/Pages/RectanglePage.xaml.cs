using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "RectangleTool"/>.
    /// </summary>
    public sealed partial class RectanglePage : Page
    {
        //@ViewModel
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }


        //@Construct
        public RectanglePage()
        {
            this.InitializeComponent();
        }
    }
}