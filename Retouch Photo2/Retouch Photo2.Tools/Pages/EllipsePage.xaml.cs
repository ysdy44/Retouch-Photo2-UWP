using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "EllipseTool"/>.
    /// </summary>
    public sealed partial class EllipsePage : Page
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        
        //@Converter
        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }
                       
        //@Construct
        public EllipsePage()
        {
            this.InitializeComponent();
            this.MoreButton.Tapped += (s, e) => this.Flyout.ShowAt(this);
        }
    }
}